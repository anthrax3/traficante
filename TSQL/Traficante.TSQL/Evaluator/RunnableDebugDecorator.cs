﻿using System.Diagnostics;
using System.IO;
using System.Threading;
using Traficante.TSQL.Evaluator.Tables;
using Traficante.TSQL.Schema;

namespace Traficante.TSQL.Evaluator
{
    public class RunnableDebugDecorator : IRunnable
    {
        private readonly IRunnable _runnable;
        private readonly string[] _filesToDelete;

        public RunnableDebugDecorator(IRunnable runnable, params string[] filesToDelete)
        {
            _runnable = runnable;
            _filesToDelete = filesToDelete;
        }

        public IEngine Provider
        {
            get => _runnable.Provider;
            set => _runnable.Provider = value;
        }

        public Table Run(CancellationToken token)
        {
            var table = _runnable.Run(token);

            foreach (var path in _filesToDelete)
            {
                var file = new FileInfo(path);

                if (file.Exists)
                    file.Delete();
            }

            return table;
        }
    }
}