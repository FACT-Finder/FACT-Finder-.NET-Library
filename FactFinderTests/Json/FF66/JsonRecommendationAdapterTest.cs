using System;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Omikron.FactFinder;
using Omikron.FactFinder.Json.FF66;
using Omikron.FactFinderTests.Utility;

namespace Omikron.FactFinderTests.Json.FF66
{
    [TestClass]
    public class JsonRecommendationAdapterTest : BaseTest
    {
        private UnixClock Clock { get; set; }
        private JsonRecommendationAdapter RecommendationAdapter { get; set; }

        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            log = LogManager.GetLogger(typeof(UrlBuilderTest));
            TestWebRequestCreate.SetupResponsePath("Responses/Json66/");
        }

        [TestInitialize]
        public override void InitializeTest()
        {
            base.InitializeTest();
            Clock = new UnixClock();
            var dataProvider = new HttpDataProvider();
            var parametersHandler = new ParametersHandler();

            RecommendationAdapter = new JsonRecommendationAdapter(dataProvider, parametersHandler);
        }

        [TestMethod]
        [ExpectedException(typeof(NotSupportedException))]
        // The recommendation engine did not expose a JSON API until FF 6.7
        public void TestGetRecommendations()
        {
            RecommendationAdapter.SetProductID("274036");
            var recommendations = RecommendationAdapter.Recommendations;
        }
    }
}
