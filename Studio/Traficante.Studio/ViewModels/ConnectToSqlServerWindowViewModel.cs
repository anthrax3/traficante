﻿using Avalonia.Controls;
using ReactiveUI;
using System;
using System.Collections.Generic;
using System.Reactive;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Traficante.Studio.Models;
using Traficante.Studio.Services;

namespace Traficante.Studio.ViewModels
{
    public class ConnectToSqlServerWindowViewModel : ViewModelBase
    {
        public ReactiveCommand<Unit, Unit> ConnectCommand { get; }
        public ReactiveCommand<Unit, Unit> CancelCommand { get; }
        public Interaction<Unit, Unit> CloseInteraction { get; } = new Interaction<Unit, Unit>();

        private SqlServerConnectionInfo _input;
        public SqlServerConnectionInfo Input
        {
            get => _input;
            set => this.RaiseAndSetIfChanged(ref _input, value);
        }

        public SqlServerConnectionInfo Output { get; set; }

        private string _connectError;
        public string ConnectError
        {
            get => _connectError;
            set => this.RaiseAndSetIfChanged(ref _connectError, value);
        }

        private readonly ObservableAsPropertyHelper<bool> _isConnecting;
        public bool IsConnecting  => _isConnecting.Value;

        private readonly ObservableAsPropertyHelper<bool> _canChangeServerAndAuthentication;
        public bool CanChangeServerAndAuthentication => _canChangeServerAndAuthentication.Value;

        private readonly ObservableAsPropertyHelper<bool> _canChangeUserIdAndPassword;
        public bool CanChangeUserIdAndPassword => _canChangeUserIdAndPassword.Value;

        public AppData AppData { get; set; }

        public ConnectToSqlServerWindowViewModel(SqlServerConnectionInfo input, AppData appData)
        {
            Input = input ?? new SqlServerConnectionInfo();
            AppData = appData;

            ConnectCommand = ReactiveCommand
                .CreateFromObservable( () =>  
                    Observable
                        .StartAsync(ct => Connect(ct))
                        .TakeUntil(this.CancelCommand));

            CancelCommand = ReactiveCommand
                .CreateFromObservable(() =>
                   Observable
                       .StartAsync(ct => Cancel()));

            //CancelCommand = ReactiveCommand
            //    .CreateFromObservable(() =>
            //        Observable
            //            .ObserveOn(RxApp.MainThreadScheduler)
            //            .StartAsync(ct => Cancel(ct)));

            ConnectCommand.IsExecuting
                .ToProperty(this, x => x.IsConnecting, out _isConnecting);

            ConnectCommand.IsExecuting
                .Select(x => x == false)
                .ToProperty(this, x => x.CanChangeServerAndAuthentication, out _canChangeServerAndAuthentication);

            Observable.Merge(
                    ConnectCommand.IsExecuting.Select(x => x == false), 
                    this.Input.WhenAnyValue(x => x.Authentication).Select(x => x == SqlServerAuthentication.SqlServer))
                .ToProperty(this, x => x.CanChangeUserIdAndPassword, out _canChangeUserIdAndPassword);
        }

        private async Task<Unit> Cancel()
        {
            if (IsConnecting == false)
                await CloseInteraction.Handle(Unit.Default);
            return Unit.Default;
        }

        private async Task<Unit> Connect(CancellationToken ct)
        {
            try
            {
                ConnectError = string.Empty;
                await new SqlServerService().TryConnectAsync(Input, ct);
                Output = Input;
                AppData.Objects.Add(new SqlServerObjectModel(Output));
                await CloseInteraction.Handle(Unit.Default);
            } catch(Exception ex)
            {
                ConnectError = ex.Message;
            }
            return Unit.Default;
        }
    }

}
