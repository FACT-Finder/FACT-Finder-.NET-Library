using System.Net;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Omikron.FactFinder.Core.Server;
using Omikron.FactFinder.Util;
using Omikron.FactFinderTests.TestUtility ;

namespace Omikron.FactFinderTests.Core.Server
{
    [TestClass]
    public class HttpDataProviderTest : BaseTest
    {
        private UnixClock Clock { get; set; }
        private HttpDataProvider DataProvider { get; set; }

        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            log = LogManager.GetLogger(typeof(HttpDataProviderTest));
            TestWebRequestCreate.SetupResponsePath("Responses/");
        }

        [TestInitialize]
        public override void InitializeTest()
        {
            base.InitializeTest();
            Clock = new UnixClock();
            DataProvider = new HttpDataProvider();

            //WebRequest.RegisterPrefix("test", new TestWebRequestCreate());
            //TestWebRequestCreate.CreateTestRequest("lol?");
        }

        [TestMethod()]
        public void TestGetData()
        {
            DataProvider.Type = RequestType.WhatsHot;
            DataProvider.SetParameter("do", "getTagCloud");
            DataProvider.SetParameter("format", "json");
            string test = DataProvider.Data;

            Assert.AreEqual(HttpStatusCode.OK, DataProvider.LastStatusCode);
        }
    }
}
