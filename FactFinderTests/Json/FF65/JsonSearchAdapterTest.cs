using System;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Omikron.FactFinder;
using Omikron.FactFinder.Data;
using Omikron.FactFinder.Json.FF65;
using Omikron.FactFinderTests.Utility;

namespace Omikron.FactFinderTests.Json.FF65
{
    [TestClass]
    public class JsonSearchAdapterTest : BaseTest
    {
        private UnixClock Clock { get; set; }
        private JsonSearchAdapter SearchAdapter { get; set; }

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
            var parametersHandler = new ParametersHandler();
            var dataProvider = new HttpDataProvider();

            SearchAdapter = new JsonSearchAdapter(dataProvider, parametersHandler);
        }

        [TestMethod]
        public void TestGetResult()
        {
            SearchAdapter.SetParameter("query", "bmx");

            var result = SearchAdapter.Result;
            Assert.AreEqual(3, result.FoundRecordsCount);
            Assert.AreEqual(1, result.Count);
            Assert.AreEqual("270863", result[0].ID);
        }

        [TestMethod]
        public void TestGetStatus()
        {
            SearchAdapter.SetParameter("query", "bmx");

            var status = SearchAdapter.SearchStatus;

            Assert.AreEqual(SearchStatus.ResultsFound, status);
        }

        [TestMethod]
        public void TestSearchTimeInfo()
        {
            SearchAdapter.SetParameter("query", "bmx");

            Assert.IsFalse(SearchAdapter.IsSearchTimedOut);
            Assert.AreEqual(100, SearchAdapter.SearchTime);
        }

        [TestMethod]
        public void TestGetAfterSearchNavigation()
        {
            SearchAdapter.SetParameter("query", "bmx");

            var asn = SearchAdapter.Asn;
            Assert.AreEqual(4, asn.Count);
            Assert.AreEqual(AsnGroupStyle.Default, asn[0].Style);
            Assert.AreEqual("Kategorie", asn[0].Name);
            Assert.AreEqual(5, asn[0].DetailedLinkCount);
            Assert.AreEqual(3, asn[0].Count);
            Assert.IsTrue(asn[0][0].Selected);
            Assert.IsTrue(asn[0][1].Selected);
            Assert.IsTrue(asn[0][2].Selected);
            Assert.AreEqual(3, asn[0][2].MatchCount);

            Assert.IsFalse(asn[1][0].Selected);
            Assert.IsFalse(asn[1][1].Selected);

            Assert.AreEqual(AsnGroupStyle.Slider, asn[3].Style);
            Assert.AreEqual("Preis", asn[3].Name);
            Assert.AreEqual("€", asn[3].Unit);
            Assert.AreEqual(10, asn[3].DetailedLinkCount);
            var slider = (AsnSliderItem)asn[3][0];
            Assert.AreEqual(20.0, slider.AbsoluteMaximum, 0.001);
            Assert.AreEqual(5.0, slider.AbsoluteMinimum, 0.001);
            Assert.AreEqual(15.95, slider.SelectedMaximum, 0.001);
            Assert.AreEqual(13.49, slider.SelectedMinimum, 0.001);
            Assert.AreEqual("products_price_min", slider.Field);
        }

        [TestMethod]
        public void TestGetProductsPerPageOptions()
        {
            SearchAdapter.SetParameter("query", "bmx");

            var pppOptions = SearchAdapter.ProductsPerPageOptions;
            Assert.AreEqual(3, pppOptions.Count);
            Assert.IsFalse(pppOptions[0].Selected);
            Assert.IsTrue(pppOptions[1].Selected);
            Assert.AreSame(pppOptions[0], pppOptions.DefaultOption);
            Assert.AreEqual(pppOptions[1], pppOptions.SelectedOption);
            Assert.AreEqual("12", pppOptions[0].Label);
        }

        [TestMethod]
        public void TestGetPaging()
        {
            SearchAdapter.SetParameter("query", "bmx");

            var paging = SearchAdapter.Paging;
            Assert.AreEqual(1, paging.CurrentPage);
            Assert.AreEqual(1, paging.PageCount);
            Assert.AreEqual(0, paging.Count);
        }

        [TestMethod]
        public void TestGetSorting()
        {
            SearchAdapter.SetParameter("query", "bmx");

            var sorting = SearchAdapter.Sorting;
            Assert.AreEqual(5, sorting.Count);
            Assert.AreEqual("sort.relevanceDescription", sorting[0].Label);
            Assert.IsTrue(sorting[0].Selected);
            Assert.IsFalse(sorting[1].Selected);
        }

        [TestMethod]
        public void TestGetBreadCrumbTrail()
        {
            SearchAdapter.SetParameter("query", "bmx");

            var breadCrumbTrail = SearchAdapter.BreadCrumbTrail;
            Assert.AreEqual(2, breadCrumbTrail.Count);
            Assert.AreEqual("bmx", breadCrumbTrail[0].Label);
            Assert.AreEqual("category1", breadCrumbTrail[1].FieldName);
        }

        [TestMethod]
        public void TestGetCampaigns()
        {
            SearchAdapter.SetParameter("query", "campaigns");

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
            Assert.AreEqual(expectedFeedback, campaigns.GetFeedbackFor("0"));

            Assert.IsTrue(campaigns.HasPushedProducts());
            var pushedProducts = campaigns.GetPushedProducts();
            Assert.AreEqual(3, pushedProducts.Count);
            Assert.AreEqual("11660", pushedProducts[0].ID);
        }
    }
}
