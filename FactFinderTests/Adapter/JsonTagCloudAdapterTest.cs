using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Omikron.FactFinder.Adapter;
using Omikron.FactFinder.Core;
using Omikron.FactFinder.Core.Client;
using Omikron.FactFinder.Core.Server;
using Omikron.FactFinder.Util;
using Omikron.FactFinderTests.TestUtility;

namespace Omikron.FactFinderTests.Adapter
{
    [TestClass]
    public class TagCloudTest : BaseTest
    {
        private UnixClock Clock { get; set; }
        private JsonTagCloudAdapter TagCloudAdapter { get; set; }

        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            log = LogManager.GetLogger(typeof(TagCloudTest));
            TestWebRequestCreate.SetupResponsePath("Responses/");
        }

        [TestInitialize]
        public override void InitializeTest()
        {
            base.InitializeTest();
            Clock = new UnixClock();
            var dataProvider = new HttpDataProvider();
            var parametersHandler = new ParametersConverter();
            var requestParser = new RequestParser();
            var clientUrlBuilder = new Omikron.FactFinder.Core.Client.UrlBuilder(requestParser);

            TagCloudAdapter = new JsonTagCloudAdapter(dataProvider, parametersHandler, clientUrlBuilder);
        }

        [TestMethod]
        public void TestGetTagCloud()
        {
            var tagCloud = TagCloudAdapter.TagCloud;

            Assert.AreEqual(5, tagCloud.Count);
            Assert.AreEqual(0.561, tagCloud[0].Weight, 0.0001);
            Assert.AreEqual(1266, tagCloud[0].SearchCount);
            Assert.AreEqual("28+zoll+damen", tagCloud[0].Label);
        }
    }
}
