using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Omikron.FactFinder;
using Omikron.FactFinderTests.Utility;
using log4net;
using System.Collections.Specialized;

namespace Omikron.FactFinderTests
{
    [TestClass]
    public class ParametersHandlerTest : BaseTest
    {
        private static ParametersHandler ParametersConverter { get; set; }

        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            log = LogManager.GetLogger(typeof(UrlBuilderTest));
            ParametersConverter = new ParametersHandler();
        }

        [TestInitialize]
        public override void InitializeTest()
        {
            base.InitializeTest();
        }        

        [TestMethod]
        public void TestGetServerRequestParameters()
        {
            var pageParameters = new NameValueCollection();

            pageParameters["keywords"] = "test";
            pageParameters["username"] = "admin";
            pageParameters["productsPerPage"] = "12";

            var expectedServerParameters = new NameValueCollection();

            expectedServerParameters["query"] = "test";
            expectedServerParameters["productsPerPage"] = "12";
            expectedServerParameters["channel"] = "de";

            var actualServerParameters = ParametersHandlerTest.ParametersConverter.ClientToServerRequestParameters(pageParameters);

            Assert.IsTrue(expectedServerParameters.NameValueCollectionEquals(actualServerParameters));
        }

        [TestMethod]
        public void TestOverwriteChannel()
        {
            var pageParameters = new NameValueCollection();

            pageParameters["channel"] = "en";

            var expectedServerParameters = new NameValueCollection();

            expectedServerParameters["channel"] = "en";

            var actualServerParameters = ParametersHandlerTest.ParametersConverter.ClientToServerRequestParameters(pageParameters);

            Assert.IsTrue(expectedServerParameters.NameValueCollectionEquals(actualServerParameters));

        }

        [TestMethod]
        public void TestGetPageRequestParameters()
        {
            var serverParameters = new NameValueCollection();

            serverParameters["query"] = "test";
            serverParameters["username"] = "admin";
            serverParameters["format"] = "xml";
            serverParameters["xml"] = "true";
            serverParameters["timestamp"] = "123456789";
            serverParameters["password"] = "test";
            serverParameters["channel"] = "de";
            serverParameters["productsPerPage"] = "12";

            var expectedPageParameters = new NameValueCollection();

            expectedPageParameters["keywords"] = "test";
            expectedPageParameters["productsPerPage"] = "12";
            expectedPageParameters["test"] = "value";

            var actualPageParameters = ParametersHandlerTest.ParametersConverter.ServerToClientRequestParameters(serverParameters);

            Assert.IsTrue(expectedPageParameters.NameValueCollectionEquals(actualPageParameters));
        }
    }
}
