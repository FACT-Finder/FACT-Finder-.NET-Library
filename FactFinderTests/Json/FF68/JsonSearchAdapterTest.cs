using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Omikron.FactFinder;
using Omikron.FactFinder.Data;
using Omikron.FactFinder.Json.FF68;
using Omikron.FactFinderTests.Utility;

namespace Omikron.FactFinderTests.Json.FF68
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
            TestWebRequestCreate.SetupResponsePath("Responses/Json68/");
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
        }

        [TestMethod]
        public void TestGetStatus()
        {
            SearchAdapter.SetParameter("query", "bmx");

            var status = SearchAdapter.SearchStatus;

            Assert.AreEqual(SearchStatus.ResultsFound, status);
        }

        [TestMethod]
        public void TestGetAfterSearchNavigation()
        {
            SearchAdapter.SetParameter("query", "bmx");

            var asn = SearchAdapter.Asn;
        }

        [TestMethod]
        public void TestGetProductsPerPageOptions()
        {
            SearchAdapter.SetParameter("query", "bmx");

            var pppOptions = SearchAdapter.ProductsPerPageOptions;
        }

        [TestMethod]
        public void TestGetPaging()
        {
            SearchAdapter.SetParameter("query", "bmx");

            var paging = SearchAdapter.Paging;
        }

        [TestMethod]
        public void TestGetSorting()
        {
            SearchAdapter.SetParameter("query", "bmx");

            var sorting = SearchAdapter.Sorting;
        }

        [TestMethod]
        public void TestGetBreadCrumbTrail()
        {
            SearchAdapter.SetParameter("query", "bmx");

            var breadCrumbTrail = SearchAdapter.BreadCrumbTrail;
        }

        [TestMethod]
        public void TestGetCampaigns()
        {
            SearchAdapter.SetParameter("query", "fahrrad");

            var campaigns = SearchAdapter.Campaigns;
        }
    }
}
