using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Omikron.FactFinder.Adapter;
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
        private TagCloud TagCloudAdapter { get; set; }

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
            var requestParser = new RequestParser();
            var requestFactory = new HttpRequestFactory(requestParser.RequestParameters);
            var clientUrlBuilder = new Omikron.FactFinder.Core.Client.UrlBuilder(requestParser);

            TagCloudAdapter = new TagCloud(requestFactory.GetRequest(), clientUrlBuilder);
        }

        [TestMethod]
        public void TestGetTagCloud()
        {
            var tagCloud = TagCloudAdapter.TagCloudData;
            Assert.AreEqual("?seoPath=%2F28%2Bzoll%2Bdamen%2Fq&channel=de", tagCloud[0].Url.OriginalString);
            Assert.AreEqual(5, tagCloud.Count);
            Assert.AreEqual(0.561, tagCloud[0].Weight, 0.0001);
            Assert.AreEqual(1266, tagCloud[0].SearchCount);
            Assert.AreEqual("28+zoll+damen", tagCloud[0].Label);
        }
    }
}
