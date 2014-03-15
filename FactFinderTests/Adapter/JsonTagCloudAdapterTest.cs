using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Omikron.FactFinder;
using Omikron.FactFinder.Json.FF69;
using Omikron.FactFinderTests.Utility;

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
            var parametersHandler = new ParametersHandler();

            TagCloudAdapter = new JsonTagCloudAdapter(dataProvider, parametersHandler);
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
