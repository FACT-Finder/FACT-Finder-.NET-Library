﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Omikron.FactFinder;
using Omikron.FactFinder.Json.FF65;
using Omikron.FactFinder.Data;

namespace Omikron.FactFinderTests.Json.FF65
{
    [TestClass]
    [DeploymentItem(@"Resources\configuration.xml", "Resources")]
    public class JsonSearchAdapterTest
    {
        private static XmlConfiguration Configuration { get; set; }
        private UnixClock Clock { get; set; }
        private JsonSearchAdapter SearchAdapter { get; set; }

        [TestInitialize()]
        public void MyTestInitialize()
        {
            Configuration = new XmlConfiguration(@"Resources\configuration.xml");
            Clock = new UnixClock();
            var parametersHandler = new ParametersHandler(Configuration);
            var dataProvider = new HttpDataProvider(Configuration);

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
            SearchAdapter.SetParameter("query", "asdasdasdasdasdasdasdasd");

            var campaigns = SearchAdapter.Campaigns;
        }
    }
}