using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Threading;
using Traficante.Studio.Services;

namespace Traficante.Studio.Tests
{
    [TestClass]
    public class SqlServerServiceTests
    {
        [TestMethod]
        public void GetSchema()
        {
            new SqlServerService().GetSchema(new SqlServerConnectionString
            {
                Server = ""
            }, CancellationToken.None).Wait();
        }
    }
}
