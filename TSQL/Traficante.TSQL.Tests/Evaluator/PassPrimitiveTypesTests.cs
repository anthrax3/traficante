﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Traficante.TSQL.Converter;
using Traficante.TSQL.Evaluator.Tests.Core.Schema;
using Traficante.TSQL.Schema;
using Traficante.TSQL.Schema.DataSources;
using Traficante.TSQL.Schema.Managers;
using Traficante.TSQL.Schema.Reflection;

namespace Traficante.TSQL.Evaluator.Tests.Core
{
    [Ignore]
    [TestClass]
    public class PassPrimitiveTypesTests : TestBase
    {
        [TestMethod]
        public void GetSchemaTableAndRowSourcePassedPrimitiveArgumentsTest()
        {
            var query = "select 1 from #test.whatever(1, 2d, true, false, 'text')";

            var vm = CreateAndRunVirtualMachine(query, new List<TestEntity>(), (passedParams) =>
            {
                Assert.AreEqual(1, passedParams[0]);
                Assert.AreEqual(2m, passedParams[1]);
                Assert.AreEqual(true, passedParams[2]);
                Assert.AreEqual(false, passedParams[3]);
                Assert.AreEqual("text", passedParams[4]);
            }, WhenCheckedParameters.OnSchemaTableOrRowSourceGet);

            vm.Run();
        }

        [TestMethod]
        public void CallWithPrimitiveArgumentsTest()
        {
            var query = "select PrimitiveArgumentsMethod(1, 2d, true, false, 'text') from #test.whatever()";

            var vm = CreateAndRunVirtualMachine(query, new List<TestEntity>(), (passedParams) =>
            {
                Assert.AreEqual(1L, passedParams[0]);
                Assert.AreEqual(2m, passedParams[1]);
                Assert.AreEqual(true, passedParams[2]);
                Assert.AreEqual(false, passedParams[3]);
                Assert.AreEqual("text", passedParams[4]);
            }, WhenCheckedParameters.OnMethodCall);

            vm.Run();
        }

        private enum WhenCheckedParameters
        {
            OnSchemaTableOrRowSourceGet,
            OnMethodCall
        }


        private class TestEntity
        {
        }

        private CompiledQuery CreateAndRunVirtualMachine(string script, IEnumerable<TestEntity> source, Action<object[]> onGetTableOrRowSource, WhenCheckedParameters whenChecked)
        {
            return new CompiledQuery(new Runner().RunAndReturnTable(script, new TSQLEngine()));
        }
    }
}
