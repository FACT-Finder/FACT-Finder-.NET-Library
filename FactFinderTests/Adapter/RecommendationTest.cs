﻿using System.Collections.Generic;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Omikron.FactFinder.Adapter;
using Omikron.FactFinder.Core.Client;
using Omikron.FactFinder.Core.Server;
using Omikron.FactFinderTests.TestUtility;

namespace Omikron.FactFinderTests.Adapter
{
    [TestClass]
    public class RecommendationTest : BaseTest
    {
        private Recommendation RecommendationAdapter { get; set; }

        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            log = LogManager.GetLogger(typeof(RecommendationTest));
            TestWebRequestCreate.SetupResponsePath("Responses/");
        }

        [TestInitialize]
        public override void InitializeTest()
        {
            base.InitializeTest();
            var requestParser = new RequestParser();
            var requestFactory = new HttpRequestFactory(requestParser.RequestParameters);
            var clientUrlBuilder = new Omikron.FactFinder.Core.Client.UrlBuilder(requestParser);

            RecommendationAdapter = new Recommendation(requestFactory.GetRequest(), clientUrlBuilder);
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