using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Omikron.FactFinder;
using Omikron.FactFinder.Json.FF68;

namespace Omikron.FactFinderTests.Json.FF68
{
    [TestClass]
    [DeploymentItem(@"Resources\configuration.xml", "Resources")]
    public class JsonTagCloudAdapterTest
    {
        private static XmlConfiguration Configuration { get; set; }
        private UnixClock Clock { get; set; }
        private JsonTagCloudAdapter TagCloudAdapter { get; set; }

        [TestInitialize()]
        public void MyTestInitialize()
        {
            Configuration = new XmlConfiguration(@"Resources\configuration.xml");
            Clock = new UnixClock();
            var dataProvider = new HttpDataProvider(Configuration);
            var parametersHandler = new ParametersHandler(Configuration);

            TagCloudAdapter = new JsonTagCloudAdapter(dataProvider, parametersHandler);
        }

        [TestMethod]
        public void TestGetTagCloud()
        {
            var tagCloud = TagCloudAdapter.TagCloud;
        }
    }
}
