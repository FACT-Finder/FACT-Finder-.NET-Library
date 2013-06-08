using System.Net;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Omikron.FactFinder;
using Omikron.FactFinderTests.Utility;

namespace Omikron.FactFinderTests
{
    [TestClass]
    public class HttpDataProviderTest : BaseTest
    {
        private UnixClock Clock { get; set; }
        private HttpDataProvider DataProvider { get; set; }

        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            log = LogManager.GetLogger(typeof(UrlBuilderTest));
            TestWebRequestCreate.SetupResponsePath("Responses/Raw/");
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
            DataProvider.Type = RequestType.TagCloud;
            DataProvider.SetParameter("do", "getTagCloud");
            DataProvider.SetParameter("format", "json");
            string test = DataProvider.Data;

            Assert.AreEqual(HttpStatusCode.OK, DataProvider.LastStatusCode);
        }
    }
}
