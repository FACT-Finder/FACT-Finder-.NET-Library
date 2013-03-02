using Microsoft.VisualStudio.TestTools.UnitTesting;
using Omikron.FactFinder;
using System.Net;

namespace Omikron.FactFinderTests
{
    [TestClass]
    public class HttpDataProviderTest
    {
        private UnixClock Clock { get; set; }
        private HttpDataProvider DataProvider { get; set; }

        [TestInitialize()]
        public void MyTestInitialize()
        {
            Clock = new UnixClock();
            DataProvider = new HttpDataProvider();

            //WebRequest.RegisterPrefix("test", new TestWebRequestCreate());
            //TestWebRequestCreate.CreateTestRequest("lol?");
        }

        [TestMethod()]
        public void TestGetData()
        {
            DataProvider.Type = RequestType.TagCloud;
            DataProvider.SetParameter("do", "getTagCloud");
            DataProvider.SetParameter("format", "json");
            string test = DataProvider.Data;

            Assert.AreEqual(HttpStatusCode.OK, DataProvider.LastStatusCode);
        }
    }
}
