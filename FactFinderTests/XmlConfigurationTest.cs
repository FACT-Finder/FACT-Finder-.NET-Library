using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Omikron.FactFinder;
using Omikron.FactFinderTests.Utility;

namespace Omikron.FactFinderTests
{
    /// <summary>
    /// Summary description for ConfigurationTest
    /// </summary>
    [TestClass]
    [DeploymentItem(@"Resources\configuration.xml", "Resources")]
    public class XmlConfigurationTest
    {
        private static XmlConfiguration Configuration { get; set; }

        public XmlConfigurationTest()
        {
            //
            // TODO: Add constructor logic here
            //
        }

        private TestContext testContextInstance;

        /// <summary>
        ///Gets or sets the test context which provides
        ///information about and functionality for the current test run.
        ///</summary>
        public TestContext TestContext
        {
            get
            {
                return testContextInstance;
            }
            set
            {
                testContextInstance = value;
            }
        }

        #region Additional test attributes
        //
        // You can use the following additional attributes as you write your tests:
        //
        // Use ClassCleanup to run code after all tests in a class have run
        // [ClassCleanup()]
        // public static void MyClassCleanup() { }
        //
        // Use TestInitialize to run code before running each test 
        // [TestInitialize()]
        // public void MyTestInitialize() { }
        //
        // Use TestCleanup to run code after each test has run
        // [TestCleanup()]
        // public void MyTestCleanup() { }
        //
        #endregion


        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            Configuration = new XmlConfiguration(@"Resources\configuration.xml");
        }

        [TestMethod]
        public void TestLoadFromFile()
        {
            Assert.IsTrue(Configuration.IsDebugModeOn);

            Assert.AreEqual(@"http", Configuration.RequestProtocol);
            Assert.AreEqual(@"demoshop.fact-finder.de", Configuration.ServerAddress);
            Assert.AreEqual(@"80", Configuration.ServerPort);
            Assert.AreEqual(@"FACT-Finder", Configuration.Context);
            Assert.AreEqual(@"de", Configuration.Channel);

            Assert.AreEqual(@"user", Configuration.User);
            Assert.AreEqual(@"userpw", Configuration.Password);
            Assert.AreEqual(AuthenticationType.Simple, Configuration.AuthenticationType);
            Assert.AreEqual(@"FACT-FINDER", Configuration.AdvancedAuthPrefix);
            Assert.AreEqual(@"FACT-FINDER", Configuration.AdvancedAuthPostfix);
            Assert.AreEqual(@"de", Configuration.Language);

            Assert.AreEqual(@"UTF-8", Configuration.PageContentEncoding);
            Assert.AreEqual(@"UTF-8", Configuration.PageUrlEncoding);
            Assert.AreEqual(@"UTF-8", Configuration.ServerUrlEncoding);
        }

        [TestMethod]
        public void TestCollectionLoading()
        {
            var expectedServerIgnore = new List<string>();

            expectedServerIgnore.Add("sid");
            expectedServerIgnore.Add("password");
            expectedServerIgnore.Add("username");
            expectedServerIgnore.Add("timestamp");

            Assert.IsTrue(expectedServerIgnore.SequenceEqual(Configuration.IgnoredServerParams));

            var expectedClientIgnore = new List<string>();

            expectedClientIgnore.Add("xml");
            expectedClientIgnore.Add("format");
            expectedClientIgnore.Add("channel");
            expectedClientIgnore.Add("password");
            expectedClientIgnore.Add("username");
            expectedClientIgnore.Add("timestamp");

            Assert.IsTrue(expectedClientIgnore.SequenceEqual(Configuration.IgnoredPageParams));

            var expectedServerRequire = new Dictionary<string, string>();

            Assert.IsTrue(expectedServerRequire.DictionaryEquals(Configuration.RequiredServerParams));

            var expectedClientRequire = new Dictionary<string, string>();

            expectedClientRequire["test"] = "value";

            Assert.IsTrue(expectedClientRequire.DictionaryEquals(Configuration.RequiredPageParams));

            var expectedServerMappings = new Dictionary<string, string>();

            expectedServerMappings["keywords"] = "query";

            Assert.IsTrue(expectedServerMappings.DictionaryEquals(Configuration.ServerMappings));

            var expectedClientMappings = new Dictionary<string, string>();

            expectedClientMappings["query"] = "keywords";

            Assert.IsTrue(expectedClientMappings.DictionaryEquals(Configuration.PageMappings));            
        }

        [TestMethod]
        public void TestGetCustomValue()
        {
            Assert.AreEqual(@"testValue", Configuration.GetCustomValue(@"testField"));
        }
    }
}