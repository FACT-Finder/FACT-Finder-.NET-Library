using System;
using System.Collections.Generic;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Omikron.FactFinder;
using Omikron.FactFinder.Json.FF67;
using Omikron.FactFinderTests.Utility;

namespace Omikron.FactFinderTests.Json.FF67
{
    [TestClass]
    public class JsonProductCampaignAdapterTest : BaseTest
    {
        private UnixClock Clock { get; set; }
        private JsonProductCampaignAdapter ProductCampaignAdapter { get; set; }

        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            log = LogManager.GetLogger(typeof(UrlBuilderTest));
            TestWebRequestCreate.SetupResponsePath("Responses/Json67/");
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
            Assert.AreEqual("277992", pushedProducts[0].ID);
            Assert.AreEqual("Katalog", pushedProducts[0].GetFieldValue("category0"));

            Assert.IsTrue(campaigns.HasActiveQuestions());
            Assert.AreEqual(1, campaigns.GetActiveQuestions().Count);
            var question = campaigns.GetActiveQuestions()[0];
            Assert.AreEqual("question text", question.Text);
            Assert.AreEqual(2, question.Answers.Count);
            Assert.AreEqual("answer text 1", question.Answers[0].Text);
            Assert.IsFalse(question.Answers[0].HasSubQuestions());
            Assert.AreEqual("answer text 2", question.Answers[1].Text);
            Assert.IsFalse(question.Answers[1].HasSubQuestions());
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
            Assert.AreEqual("234567", pushedProducts[0].ID);
            Assert.AreEqual("Katalog", pushedProducts[0].GetFieldValue("category0"));

            Assert.IsTrue(campaigns.HasActiveQuestions());
            Assert.AreEqual(1, campaigns.GetActiveQuestions().Count);
            var question = campaigns.GetActiveQuestions()[0];
            Assert.AreEqual("question text", question.Text);
            Assert.AreEqual(2, question.Answers.Count);
            Assert.AreEqual("answer text 1", question.Answers[0].Text);
            Assert.IsFalse(question.Answers[0].HasSubQuestions());
            Assert.AreEqual("answer text 2", question.Answers[1].Text);
            Assert.IsFalse(question.Answers[1].HasSubQuestions());
        }
    }
}
