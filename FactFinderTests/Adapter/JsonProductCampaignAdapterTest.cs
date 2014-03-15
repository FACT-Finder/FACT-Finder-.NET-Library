using System;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Omikron.FactFinder;
using Omikron.FactFinder.Adapter;
using Omikron.FactFinder.Util;
using Omikron.FactFinderTests.Utility;

namespace Omikron.FactFinderTests.Adapter
{
    [TestClass]
    public class ProductCampaignTest : BaseTest
    {
        private UnixClock Clock { get; set; }
        private JsonProductCampaignAdapter ProductCampaignAdapter { get; set; }

        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            log = LogManager.GetLogger(typeof(ProductCampaignTest));
            TestWebRequestCreate.SetupResponsePath("Responses/");
        }

        [TestInitialize]
        public override void InitializeTest()
        {
            base.InitializeTest();
            Clock = new UnixClock();
            var dataProvider = new HttpDataProvider();
            var parametersHandler = new ParametersHandler();

            ProductCampaignAdapter = new JsonProductCampaignAdapter(dataProvider, parametersHandler);
        }

        [TestMethod]
        public void TestProductDetailCampaigns()
        {
            var productIDs = new string[] {
                "123"
            };
            ProductCampaignAdapter.SetProductIDs(productIDs);
            ProductCampaignAdapter.MakeProductDetailCampaign();
            var campaigns = ProductCampaignAdapter.Campaigns;

            Assert.IsTrue(campaigns.HasRedirect());
            Assert.AreEqual(new Uri("http://www.fact-finder.de"), campaigns.GetRedirectUrl());

            Assert.IsTrue(campaigns.HasFeedback());
            var expectedFeedback = "test feedback" + System.Environment.NewLine;
            Assert.AreEqual(expectedFeedback, campaigns.GetFeedbackFor("html header"));
            Assert.AreEqual(expectedFeedback, campaigns.GetFeedbackFor("9"));

            Assert.IsTrue(campaigns.HasPushedProducts());
            var pushedProducts = campaigns.GetPushedProducts();
            Assert.AreEqual(1, pushedProducts.Count);
            Assert.AreEqual("221910", pushedProducts[0].ID);
            Assert.AreEqual("KHE", pushedProducts[0].GetFieldValue("Brand"));

            Assert.IsFalse(campaigns.HasActiveQuestions());
        }

        [TestMethod]
        public void TestShoppingCartCampaigns()
        {
            var productIDs = new string[] {
                "456",
                "789"
            };
            ProductCampaignAdapter.SetProductIDs(productIDs);
            ProductCampaignAdapter.MakeShoppingCartCampaign();
            var campaigns = ProductCampaignAdapter.Campaigns;

            Assert.IsTrue(campaigns.HasRedirect());
            Assert.AreEqual(new Uri("http://www.fact-finder.de"), campaigns.GetRedirectUrl());

            Assert.IsTrue(campaigns.HasFeedback());
            var expectedFeedback = "test feedback" + System.Environment.NewLine;
            Assert.AreEqual(expectedFeedback, campaigns.GetFeedbackFor("html header"));
            Assert.AreEqual(expectedFeedback, campaigns.GetFeedbackFor("9"));

            Assert.IsTrue(campaigns.HasPushedProducts());
            var pushedProducts = campaigns.GetPushedProducts();
            Assert.AreEqual(1, pushedProducts.Count);
            Assert.AreEqual("221910", pushedProducts[0].ID);
            Assert.AreEqual("KHE", pushedProducts[0].GetFieldValue("Brand"));

            Assert.IsFalse(campaigns.HasActiveQuestions());
        }
    }
}
