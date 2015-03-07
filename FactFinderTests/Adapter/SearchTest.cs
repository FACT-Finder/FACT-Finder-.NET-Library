using System;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Omikron.FactFinder.Adapter;
using Omikron.FactFinder.Core.Client;
using Omikron.FactFinder.Core.Server;
using Omikron.FactFinder.Data;
using Omikron.FactFinderTests.TestUtility;

namespace Omikron.FactFinderTests.Adapter
{
    [TestClass]
    public class SearchTest : BaseTest
    {
        private Search SearchAdapter { get; set; }

        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            log = LogManager.GetLogger(typeof(SearchTest));
            TestWebRequestCreate.SetupResponsePath("Responses/");
        }

        [TestInitialize]
        public override void InitializeTest()
        {
            base.InitializeTest();
            var requestParser = new RequestParser();
            var requestFactory = new HttpRequestFactory(requestParser.RequestParameters);
            var clientUrlBuilder = new Omikron.FactFinder.Core.Client.UrlBuilder(requestParser);

            SearchAdapter = new Search(requestFactory.GetRequest(), clientUrlBuilder);

            SearchAdapter.SetQuery("bmx");
        }

        [TestMethod]
        public void TestGetResult()
        {
            var result = SearchAdapter.Result;
            Assert.AreEqual(66, result.FoundRecordsCount);
            Assert.AreEqual("WOwfiHGNS", result.RefKey);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("278003", result[0].ID);
        }

        [TestMethod]
        public void TestGetStatus()
        {
            var status = SearchAdapter.SearchStatus;

            Assert.AreEqual(SearchStatus.ResultsFound, status);
        }

        [TestMethod]
        public void TestSearchTimeInfo()
        {
            Assert.IsFalse(SearchAdapter.IsSearchTimedOut);
            Assert.AreEqual(100, SearchAdapter.SearchTime);
        }

        [TestMethod]
        public void TestGetAfterSearchNavigation()
        {
            var asn = SearchAdapter.Asn;
            Assert.AreEqual(4, asn.Count);
            
            Assert.AreEqual(AsnGroupStyle.Default, asn[0].Style);
            Assert.AreEqual("Für Wen?", asn[0].Name);
            Assert.AreEqual(5, asn[0].DetailedLinkCount);
            Assert.AreEqual(3, asn[0].Count);
            Assert.IsFalse(asn[0][0].Selected);
            Assert.IsFalse(asn[0][1].Selected);
            Assert.IsFalse(asn[0][2].Selected);
            Assert.AreEqual(1, asn[0][2].MatchCount);
            
            Assert.AreEqual(AsnGroupStyle.Tree, asn[1].Style);
            Assert.AreEqual("Kategorie", asn[1].Name);
            Assert.AreEqual(5, asn[1].DetailedLinkCount);
            Assert.AreEqual(3, asn[1].Count);
            Assert.IsTrue(asn[1][0].Selected);
            Assert.IsTrue(asn[1][1].Selected);
            Assert.IsTrue(asn[1][2].Selected);
            Assert.AreEqual(0, asn[1][2].MatchCount);

            Assert.AreEqual(AsnGroupStyle.MultiSelect, asn[2].Style);
            Assert.IsFalse(asn[2][0].Selected);
            Assert.IsFalse(asn[2][1].Selected);
            Assert.IsFalse(asn[2][2].Selected);

            Assert.AreEqual(AsnGroupStyle.Slider, asn[3].Style);
            Assert.AreEqual("Bewertung", asn[3].Name);
            Assert.AreEqual("", asn[3].Unit);
            Assert.AreEqual(5, asn[3].DetailedLinkCount);
            var slider = (AsnSliderItem)asn[3][0];
            Assert.AreEqual(5, slider.AbsoluteMaximum, 0.001);
            Assert.AreEqual(4, slider.AbsoluteMinimum, 0.001);
            Assert.AreEqual(5, slider.SelectedMaximum, 0.001);
            Assert.AreEqual(4, slider.SelectedMinimum, 0.001);
            Assert.AreEqual("RatingAverage", slider.Field);
        }

        [TestMethod]
        public void TestGetProductsPerPageOptions()
        {
            var pppOptions = SearchAdapter.ProductsPerPageOptions;
            Assert.AreEqual(3, pppOptions.Count);
            Assert.IsFalse(pppOptions[0].Selected);
            Assert.IsTrue(pppOptions[1].Selected);
            Assert.AreSame(pppOptions[0], pppOptions.DefaultOption);
            Assert.AreSame(pppOptions[1], pppOptions.SelectedOption);
            Assert.AreEqual("12", pppOptions[0].Label);
        }

        [TestMethod]
        public void TestGetPaging()
        {
            var paging = SearchAdapter.Paging;
            Assert.AreEqual(1, paging.CurrentPage);
            Assert.AreEqual(6, paging.PageCount);
            Assert.AreEqual(6, paging.Count);
        }

        [TestMethod]
        public void TestGetSorting()
        {
            var sorting = SearchAdapter.Sorting;
            Assert.AreEqual(5, sorting.Count);
            Assert.AreEqual("sort.relevanceDescription", sorting[0].Label);
            Assert.IsTrue(sorting[0].Selected);
            Assert.IsFalse(sorting[1].Selected);
        }

        [TestMethod]
        public void TestGetBreadCrumbTrail()
        {
            var breadCrumbTrail = SearchAdapter.BreadCrumbTrail;
            Assert.AreEqual(3, breadCrumbTrail.Count);
            Assert.AreEqual("bmx", breadCrumbTrail[0].Label);
            Assert.AreEqual("Category1", breadCrumbTrail[1].FieldName);
        }

        [TestMethod]
        public void TestEmptyCampaigns()
        {
            Assert.AreEqual(0, SearchAdapter.Campaigns.Count);
        }

        [TestMethod]
        public void TestGetCampaigns()
        {
            SearchAdapter.SetQuery("campaigns");

            var campaigns = SearchAdapter.Campaigns;

            Assert.IsTrue(campaigns.HasRedirect());
            Assert.AreEqual(new Uri("http://www.fact-finder.de"), campaigns.GetRedirectUrl());

            Assert.IsTrue(campaigns.HasFeedback());
            var expectedFeedback = String.Join(System.Environment.NewLine, new string[3]
            {
                "test feedback 1",
                "test feedback 2",
                ""
            });
            Assert.AreEqual(expectedFeedback, campaigns.GetFeedbackFor("html header"));
            Assert.AreEqual(expectedFeedback, campaigns.GetFeedbackFor("9"));
            expectedFeedback = "test feedback 3" + System.Environment.NewLine;
            Assert.AreEqual(expectedFeedback, campaigns.GetFeedbackFor("below header"));
            Assert.AreEqual(expectedFeedback, campaigns.GetFeedbackFor("6"));

            Assert.IsTrue(campaigns.HasPushedProducts());
            var pushedProducts = campaigns.GetPushedProducts();
            Assert.AreEqual(1, pushedProducts.Count);
            Assert.AreEqual("17552", pushedProducts[0].ID);
            Assert.AreEqual(@"..Fahrräder..", pushedProducts[0].GetFieldValue("Category1"));

            Assert.IsTrue(campaigns.HasActiveQuestions());
            Assert.AreEqual(1, campaigns.GetActiveQuestions().Count);
            var question = campaigns.GetActiveQuestions()[0];
            Assert.AreEqual("question text", question.Text);
            Assert.AreEqual(2, question.Answers.Count);
            Assert.AreEqual("answer text 1", question.Answers[0].Text);
            Assert.AreEqual(0, question.Answers[0].SubQuestions.Count);
            Assert.AreEqual("answer text 2", question.Answers[1].Text);
            Assert.AreEqual(0, question.Answers[0].SubQuestions.Count);
        }
    }
}
