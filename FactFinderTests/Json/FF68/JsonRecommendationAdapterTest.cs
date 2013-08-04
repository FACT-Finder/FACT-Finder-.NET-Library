using System.Collections.Generic;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Omikron.FactFinder;
using Omikron.FactFinder.Json.FF68;
using Omikron.FactFinderTests.Utility;

namespace Omikron.FactFinderTests.Json.FF68
{
    [TestClass]
    public class JsonRecommendationAdapterTest : BaseTest
    {
        private UnixClock Clock { get; set; }
        private JsonRecommendationAdapter RecommendationAdapter { get; set; }

        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            log = LogManager.GetLogger(typeof(JsonRecommendationAdapterTest));
            TestWebRequestCreate.SetupResponsePath("Responses/Json68/");
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
        public void TestGetRecommendations()
        {
            RecommendationAdapter.SetProductID("274036");
            var recommendations = RecommendationAdapter.Recommendations;
            Assert.AreEqual(1, recommendations.FoundRecordsCount);
            Assert.AreEqual("274035", recommendations[0].ID);
        }

        [TestMethod]
        public void TestIDsOnly()
        {
            RecommendationAdapter.SetProductID("274036");
            RecommendationAdapter.IDsOnly = true;
            var recommendations = RecommendationAdapter.Recommendations;
            Assert.AreEqual(1, recommendations.FoundRecordsCount);
            Assert.AreEqual("274035", recommendations[0].ID);
        }

        [TestMethod]
        public void TestReload()
        {
            RecommendationAdapter.SetProductID("274036");
            var recommendations = RecommendationAdapter.Recommendations;
            Assert.AreEqual("274035", recommendations[0].ID);
            RecommendationAdapter.SetProductID("233431");
            recommendations = RecommendationAdapter.Recommendations;
            Assert.AreEqual("327212", recommendations[0].ID);
        }

        [TestMethod]
        public void TestReloadAfterIDsOnly()
        {
            RecommendationAdapter.SetProductID("274036");
            RecommendationAdapter.IDsOnly = true;
            var recommendations = RecommendationAdapter.Recommendations;
            RecommendationAdapter.IDsOnly = false;
            recommendations = RecommendationAdapter.Recommendations;
            Assert.IsNotNull(recommendations[0].GetFieldValue("Description"));
        }

        [TestMethod]
        public void TestMultipleProducts()
        {
            var productIDs = new List<string>()
            {
                "274036",
                "233431"
            };
            RecommendationAdapter.SetProductIDs(productIDs);
            var recommendations = RecommendationAdapter.Recommendations;
            Assert.AreEqual(1, recommendations.FoundRecordsCount);
            Assert.AreEqual("225052", recommendations[0].ID);
        }
    }
}