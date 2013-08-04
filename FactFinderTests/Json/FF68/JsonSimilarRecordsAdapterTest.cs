using System;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Omikron.FactFinder;
using Omikron.FactFinder.Json.FF68;
using Omikron.FactFinderTests.Utility;

namespace Omikron.FactFinderTests.Json.FF68
{
    [TestClass]
    public class JsonSimilarRecordsAdapterTest : BaseTest
    {
        private UnixClock Clock { get; set; }
        private JsonSimilarRecordsAdapter SimilarRecordsAdapter { get; set; }

        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            log = LogManager.GetLogger(typeof(JsonSimilarRecordsAdapterTest));
            TestWebRequestCreate.SetupResponsePath("Responses/Json68/");
        }

        [TestInitialize]
        public override void InitializeTest()
        {
            base.InitializeTest();
            Clock = new UnixClock();
            var dataProvider = new HttpDataProvider();
            var parametersHandler = new ParametersHandler();

            SimilarRecordsAdapter = new JsonSimilarRecordsAdapter(dataProvider, parametersHandler);
        }

        [TestMethod]
        public void TestGetRecords()
        {
            SimilarRecordsAdapter.SetProductID("123");
            var records = SimilarRecordsAdapter.Records;

            Assert.AreEqual(10, records.Count);
            Assert.AreEqual("293398", records[0].ID);
            Assert.AreEqual("..Fahrräder..", records[0].GetFieldValue("Category1"));
        }

        [TestMethod]
        public void TestIDsOnly()
        {
            SimilarRecordsAdapter.SetProductID("123");
            SimilarRecordsAdapter.IDsOnly = true;
            var recordIDs = SimilarRecordsAdapter.Records;

            Assert.AreEqual(10, recordIDs.Count);
            Assert.AreEqual("232670", recordIDs[0].ID);
        }

        [TestMethod]
        public void TestRecordsAfterIDsOnly()
        {
            SimilarRecordsAdapter.SetProductID("123");
            SimilarRecordsAdapter.IDsOnly = true;
            var recordIDs = SimilarRecordsAdapter.Records;
            SimilarRecordsAdapter.IDsOnly = false;
            var records = SimilarRecordsAdapter.Records;

            Assert.AreEqual(10, recordIDs.Count);
            Assert.AreEqual("232670", recordIDs[0].ID);
            Assert.AreEqual(10, records.Count);
            Assert.AreEqual("293398", records[0].ID);
            Assert.AreEqual("..Fahrräder..", records[0].GetFieldValue("Category1"));
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

            Assert.AreEqual(6, attributes.Count);
            Assert.AreEqual("..Fahrräder..", attributes["Category1"]);
        }
    }
}
