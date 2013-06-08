using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Omikron.FactFinder;
using Omikron.FactFinder.Json.FF65;
using Omikron.FactFinderTests.Utility;

namespace Omikron.FactFinderTests.Json.FF65
{
    [TestClass]
    public class JsonSuggestAdapterTest : BaseTest
    {
        private UnixClock Clock { get; set; }
        private JsonSuggestAdapter SuggestAdapter { get; set; }

        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            log = LogManager.GetLogger(typeof(UrlBuilderTest));
            TestWebRequestCreate.SetupResponsePath("Responses/Json65/");
        }

        [TestInitialize]
        public override void InitializeTest()
        {
            base.InitializeTest();
            Clock = new UnixClock();
            var dataProvider = new HttpDataProvider();
            var parametersHandler = new ParametersHandler();

            SuggestAdapter = new JsonSuggestAdapter(dataProvider, parametersHandler);
        }

        [TestMethod]
        public void TestGetSuggestions()
        {
            SuggestAdapter.SetParameter("query", "bmx");
            //var suggestions = SuggestAdapter.Suggestions;
        }
    }
}
