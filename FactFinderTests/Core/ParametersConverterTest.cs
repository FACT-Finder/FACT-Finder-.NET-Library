﻿using System.Collections.Specialized;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Omikron.FactFinder.Core;
using Omikron.FactFinderTests.TestUtility;

namespace Omikron.FactFinderTests.Core
{
    [TestClass]
    public class ParametersConverterTest : BaseTest
    {
        private static ParametersConverter ParametersConverter { get; set; }

        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            log = LogManager.GetLogger(typeof(ParametersConverterTest));
            ParametersConverter = new ParametersConverter();
        }

        [TestInitialize]
        public override void InitializeTest()
        {
            base.InitializeTest();
        }        

        [TestMethod]
        public void TestGetServerRequestParameters()
        {
            var pageParameters = new NameValueCollection() {
                {"keywords", "test"},
                {"username", "admin"},
                {"productsPerPage", "12"},
            };

            var expectedServerParameters = new NameValueCollection() {
                {"query", "test"},
                {"productsPerPage", "12"},
                {"channel", "de"},
            };

            var actualServerParameters = ParametersConverterTest.ParametersConverter.ClientToServerRequestParameters(pageParameters);

            Assert.IsTrue(expectedServerParameters.NameValueCollectionEquals(actualServerParameters));
        }

        [TestMethod]
        public void TestOverwriteChannel()
        {
            var pageParameters = new NameValueCollection();

            pageParameters["channel"] = "en";

            var expectedServerParameters = new NameValueCollection();

            expectedServerParameters["channel"] = "en";

            var actualServerParameters = ParametersConverterTest.ParametersConverter.ClientToServerRequestParameters(pageParameters);

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

            var actualPageParameters = ParametersConverterTest.ParametersConverter.ServerToClientRequestParameters(serverParameters);

            Assert.IsTrue(expectedPageParameters.NameValueCollectionEquals(actualPageParameters));
        }
    }
}
