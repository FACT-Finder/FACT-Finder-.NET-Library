using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Omikron.FactFinder;
using Omikron.FactFinder.Json.FF67;

namespace Omikron.FactFinderTests.Json.FF67
{
    [TestClass]
    [DeploymentItem(@"Resources\configuration.xml", "Resources")]
    public class JsonSuggestAdapterTest
    {
        private UnixClock Clock { get; set; }
        private JsonSuggestAdapter SuggestAdapter { get; set; }

        [TestInitialize()]
        public void MyTestInitialize()
        {
            Clock = new UnixClock();
            var dataProvider = new HttpDataProvider();
            var parametersHandler = new ParametersHandler();

            SuggestAdapter = new JsonSuggestAdapter(dataProvider, parametersHandler);
        }

        [TestMethod]
        public void TestGetSuggestions()
        {
            SuggestAdapter.SetParameter("query", "bmx");
            //var suggestions = SuggestAdapter.Suggestions;
        }
    }
}
