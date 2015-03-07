using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Omikron.FactFinder.Adapter;
using Omikron.FactFinder.Core.Client;
using Omikron.FactFinder.Core.Server;
using Omikron.FactFinderTests.TestUtility;

namespace Omikron.FactFinderTests.Adapter
{
    [TestClass]
    public class SuggestTest : BaseTest
    {
        private Suggest SuggestAdapter { get; set; }

        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            log = LogManager.GetLogger(typeof(SuggestTest));
            TestWebRequestCreate.SetupResponsePath("Responses/");
        }

        [TestInitialize]
        public override void InitializeTest()
        {
            base.InitializeTest();
            var requestParser = new RequestParser();
            var parameters = requestParser.RequestParameters;

            parameters["query"] = "bmx";

            var requestFactory = new HttpRequestFactory(parameters);
            var clientUrlBuilder = new Omikron.FactFinder.Core.Client.UrlBuilder(requestParser);

            SuggestAdapter = new Suggest(requestFactory.GetRequest(), clientUrlBuilder);
        }

        [TestMethod]
        public void TestGetSuggestions()
        {
            var suggestions = SuggestAdapter.Suggestions;

            Assert.AreEqual(3, suggestions.Count);
            Assert.AreEqual("Verde BMX", suggestions[0].Query);
            Assert.AreEqual("8blKVw-P5", suggestions[0].RefKey);
            Assert.AreEqual("brand", suggestions[0].Type);
            Assert.AreEqual("category", suggestions[1].Type);
            Assert.AreEqual("productName", suggestions[2].Type);
        }
    }
}
