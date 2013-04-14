using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Omikron.FactFinder;
using Omikron.FactFinder.Json.FF68;
using log4net;

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
            log = LogManager.GetLogger(typeof(UrlBuilderTest));
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
            RecommendationAdapter.SetProductID(274036);
            var recommendations = RecommendationAdapter.Recommendations;
            Assert.AreEqual(10, recommendations.FoundRecordsCount);
        }

        [TestMethod]
        public void TestIDsOnly()
        {
            RecommendationAdapter.SetProductID(274036);
            RecommendationAdapter.IDsOnly = true;
            var recommendations = RecommendationAdapter.Recommendations;
            Assert.AreEqual(10, recommendations.FoundRecordsCount);
        }

        [TestMethod]
        public void TestReload()
        {
            RecommendationAdapter.SetProductID(274036);
            var recommendations = RecommendationAdapter.Recommendations;
            string firstID = recommendations[0].ID;
            RecommendationAdapter.SetProductID(233431);
            recommendations = RecommendationAdapter.Recommendations;
            string secondID = recommendations[0].ID;
            Assert.IsTrue(firstID != secondID);
        }

        [TestMethod]
        public void TestReloadAfterIDsOnly()
        {
            RecommendationAdapter.SetProductID(274036);
            RecommendationAdapter.IDsOnly = true;
            var recommendations = RecommendationAdapter.Recommendations;
            RecommendationAdapter.IDsOnly = false;
            recommendations = RecommendationAdapter.Recommendations;
            Assert.IsNotNull(recommendations[0].GetFieldValue("Price"));
        }

        [TestMethod]
        public void TestMaximumResults()
        {
            RecommendationAdapter.SetProductID(274036);
            RecommendationAdapter.MaxResults = 5;
            var recommendations = RecommendationAdapter.Recommendations;
            Assert.AreEqual(5, recommendations.FoundRecordsCount);
        }
    }
}
