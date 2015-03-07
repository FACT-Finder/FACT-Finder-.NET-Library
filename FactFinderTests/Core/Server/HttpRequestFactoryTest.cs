using System.Collections.Specialized;
using System.Net;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Omikron.FactFinder.Core.Server;
using Omikron.FactFinderTests.TestUtility;

namespace Omikron.FactFinderTests.Core.Server
{
    [TestClass]
    public class HttpRequestFactoryTest : BaseTest
    {
        private HttpRequestFactory RequestFactory { get; set; }

        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            log = LogManager.GetLogger(typeof(HttpRequestFactoryTest));
            TestWebRequestCreate.SetupResponsePath("Responses/");
        }

        [TestInitialize]
        public override void InitializeTest()
        {
            base.InitializeTest();
            var parameters = new NameValueCollection()
            {
                {"test", "test"}
            };
            RequestFactory = new HttpRequestFactory(parameters);
        }

        [TestMethod()]
        public void TestLoadResponse()
        {
            var request = RequestFactory.GetRequest();

            var parameters = request.Parameters;

            parameters["format"] = "json";
            parameters["do"] = "getTagCloud";

            request.Action = RequestType.TagCloud;

            var response = request.Response;

            Assert.AreEqual(HttpStatusCode.OK, response.StatusCode);
            Assert.AreEqual("test response", response.Content);
        }
    }
}
