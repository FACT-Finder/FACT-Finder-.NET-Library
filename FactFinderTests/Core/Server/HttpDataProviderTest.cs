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
            var urlBuilder = new UrlBuilder(new UnixClock());
            DataProvider = new HttpDataProvider(urlBuilder);

            //WebRequest.RegisterPrefix("test", new TestWebRequestCreate());
            //TestWebRequestCreate.CreateTestRequest("lol?");
        }

        [TestMethod()]
        public void TestLoadResponse()
        {
            var connectionData = new ConnectionData();
            int id = DataProvider.Register(connectionData);

            var parameters = connectionData.Parameters;

            parameters["format"] = "json";
            parameters["do"] = "getTagCloud";
            parameters["test"] = "test";

            connectionData.Action = RequestType.TagCloud;

            DataProvider.LoadResponse(id);

            var response = connectionData.Response;

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("test response", response.Content);
        }
    }
}
