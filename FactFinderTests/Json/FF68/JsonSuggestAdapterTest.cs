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
    public class JsonSuggestAdapterTest : BaseTest
    {
        private UnixClock Clock { get; set; }
        private JsonSuggestAdapter SuggestAdapter { get; set; }

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

            SuggestAdapter = new JsonSuggestAdapter(dataProvider, parametersHandler);
        }

        [TestMethod]
        public void TestGetSuggestions()
        {
            SuggestAdapter.SetParameter("query", "bmx");
            var suggestions = SuggestAdapter.Suggestions;
        }
    }
}
