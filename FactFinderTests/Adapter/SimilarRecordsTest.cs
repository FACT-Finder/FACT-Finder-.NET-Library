using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Omikron.FactFinder.Adapter;
using Omikron.FactFinder.Core.Client;
using Omikron.FactFinder.Core.Server;
using Omikron.FactFinderTests.TestUtility;

namespace Omikron.FactFinderTests.Adapter
{
    [TestClass]
    public class SimilarRecordsTest : BaseTest
    {
        private SimilarRecords SimilarRecordsAdapter { get; set; }

        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            log = LogManager.GetLogger(typeof(SimilarRecordsTest));
            TestWebRequestCreate.SetupResponsePath("Responses/");
        }

        [TestInitialize]
        public override void InitializeTest()
        {
            base.InitializeTest();
            var requestParser = new RequestParser();
            var requestFactory = new HttpRequestFactory(requestParser.RequestParameters);
            var clientUrlBuilder = new Omikron.FactFinder.Core.Client.UrlBuilder(requestParser);

            SimilarRecordsAdapter = new SimilarRecords(requestFactory.GetRequest(), clientUrlBuilder);
        }

        [TestMethod]
        public void TestGetRecords()
        {
            SimilarRecordsAdapter.SetProductID("123");
            var records = SimilarRecordsAdapter.Records;

            Assert.AreEqual(6, records.Count);
            Assert.AreEqual("221911", records[0].ID);
            Assert.AreEqual("..BMX Bikes..", records[0].GetFieldValue("Category3"));
        }

        [TestMethod]
        public void TestIDsOnly()
        {
            SimilarRecordsAdapter.SetProductID("123");
            SimilarRecordsAdapter.IDsOnly = true;
            var recordIDs = SimilarRecordsAdapter.Records;

            Assert.AreEqual(6, recordIDs.Count);
            Assert.AreEqual("278006", recordIDs[0].ID);
        }

        [TestMethod]
        public void TestRecordsAfterIDsOnly()
        {
            SimilarRecordsAdapter.SetProductID("123");
            SimilarRecordsAdapter.IDsOnly = true;
            var recordIDs = SimilarRecordsAdapter.Records;
            SimilarRecordsAdapter.IDsOnly = false;
            var records = SimilarRecordsAdapter.Records;

            Assert.AreEqual(6, recordIDs.Count);
            Assert.AreEqual("278006", recordIDs[0].ID);
            Assert.AreEqual(6, records.Count);
            Assert.AreEqual("221911", records[0].ID);
            Assert.AreEqual("..BMX Bikes..", records[0].GetFieldValue("Category3"));
        }

        [TestMethod]
        public void TestMaxRecordCount()
        {
            SimilarRecordsAdapter.SetProductID("123");
            SimilarRecordsAdapter.IDsOnly = true;
            SimilarRecordsAdapter.MaxRecordCount = 3;
            var recordIDs = SimilarRecordsAdapter.Records;

            Assert.AreEqual(3, recordIDs.Count);
        }

        [TestMethod]
        public void TestSimilarAttributes()
        {
            SimilarRecordsAdapter.SetProductID("123");
            var attributes = SimilarRecordsAdapter.SimilarAttributes;

            Assert.AreEqual(3, attributes.Count);
            Assert.AreEqual("..BMX Bikes..", attributes["Category3"]);
        }
    }
}
