using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Omikron.FactFinder;
using Omikron.FactFinder.Json.FF69;
using Omikron.FactFinderTests.Utility;

namespace Omikron.FactFinderTests.Json.FF69
{
    [TestClass]
    public class JsonCompareAdapterTest : BaseTest
    {
        private UnixClock Clock { get; set; }
        private JsonCompareAdapter CompareAdapter { get; set; }

        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            log = LogManager.GetLogger(typeof(JsonCompareAdapterTest));
            TestWebRequestCreate.SetupResponsePath("Responses/Json69/");
        }

        [TestInitialize]
        public override void InitializeTest()
        {
            base.InitializeTest();
            Clock = new UnixClock();
            var dataProvider = new HttpDataProvider();
            var parametersHandler = new ParametersHandler();

            CompareAdapter = new JsonCompareAdapter(dataProvider, parametersHandler);
        }

        [TestMethod]
        public void TestGetComparison()
        {
            string[] productIDs = {
                "123",
                "456",
                "789"
            };

            CompareAdapter.SetProductIDs(productIDs);
            var records = CompareAdapter.ComparedRecords;

            Assert.AreEqual(3, records.Count);
            Assert.AreEqual("123", records[0].ID);
            Assert.AreEqual("..schwarz..", records[0].GetFieldValue("Farbe"));
        }

        [TestMethod]
        public void TestIDsOnly()
        {
            string[] productIDs = {
                "123",
                "456",
                "789"
            };

            CompareAdapter.SetProductIDs(productIDs);
            CompareAdapter.IDsOnly = true;
            var records = CompareAdapter.ComparedRecords;

            Assert.AreEqual(3, records.Count);
            Assert.AreEqual("123", records[0].ID);
        }

        [TestMethod]
        public void TestGetAttributes()
        {
            string[] productIDs = {
                "123",
                "456",
                "789"
            };

            CompareAdapter.SetProductIDs(productIDs);
            var attributes = CompareAdapter.ComparableAttributes;

            Assert.AreEqual(7, attributes.Count);
            Assert.IsFalse(attributes["Hersteller"]);
            Assert.IsTrue(attributes["Farbe"]);
            Assert.IsTrue(attributes["Material"]);
            Assert.IsFalse(attributes["Modelljahr"]);
        }
    }
}
