using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Omikron.FactFinder;
using Omikron.FactFinder.Json.FF66;
using Omikron.FactFinderTests.Utility;

namespace Omikron.FactFinderTests.Json.FF66
{
    [TestClass]
    public class JsonTagCloudAdapterTest : BaseTest
    {
        private UnixClock Clock { get; set; }
        private JsonTagCloudAdapter TagCloudAdapter { get; set; }

        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            log = LogManager.GetLogger(typeof(UrlBuilderTest));
            TestWebRequestCreate.SetupResponsePath("Responses/Json66/");
        }

        [TestInitialize]
        public override void InitializeTest()
        {
            base.InitializeTest();
            Clock = new UnixClock();
            var dataProvider = new HttpDataProvider();
            var parametersHandler = new ParametersHandler();

            TagCloudAdapter = new JsonTagCloudAdapter(dataProvider, parametersHandler);
        }

        [TestMethod]
        public void TestGetTagCloud()
        {
            var tagCloud = TagCloudAdapter.TagCloud;

            Assert.AreEqual(5, tagCloud.Count);
            Assert.AreEqual(0.0196, tagCloud[0].Weight, 0.00001);
            Assert.AreEqual(265, tagCloud[0].SearchCount);
            Assert.AreEqual("26 zoll", tagCloud[0].Label);
        }
    }
}
