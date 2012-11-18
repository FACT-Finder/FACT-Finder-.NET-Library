﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Omikron.FactFinder;
using Omikron.FactFinderTests.Utility;

namespace Omikron.FactFinderTests
{
    [TestClass]
    [DeploymentItem(@"Resources\configuration.xml", "Resources")]
    public class ParametersHandlerTest
    {
        private static ParametersHandler ParametersConverter { get; set; }

        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            var configuration = new XmlConfiguration(@"Resources\configuration.xml");
            ParametersConverter = new ParametersHandler(configuration);
        }

        [TestMethod]
        public void TestGetServerRequestParameters()
        {
            var pageParameters = new Dictionary<string, string>();

            pageParameters["keywords"] = "test";
            pageParameters["username"] = "admin";
            pageParameters["productsPerPage"] = "12";

            var expectedServerParameters = new Dictionary<string, string>();

            expectedServerParameters["query"] = "test";
            expectedServerParameters["productsPerPage"] = "12";
            expectedServerParameters["channel"] = "de";

            var actualServerParameters = ParametersHandlerTest.ParametersConverter.GetServerRequestParams(pageParameters);

            Assert.IsTrue(expectedServerParameters.DictionaryEquals(actualServerParameters));
        }

        [TestMethod]
        public void TestOverwriteChannel()
        {
            var pageParameters = new Dictionary<string, string>();

            pageParameters["channel"] = "en";

            var expectedServerParameters = new Dictionary<string, string>();

            expectedServerParameters["channel"] = "en";

            var actualServerParameters = ParametersHandlerTest.ParametersConverter.GetServerRequestParams(pageParameters);

            Assert.IsTrue(expectedServerParameters.DictionaryEquals(actualServerParameters));

        }

        [TestMethod]
        public void TestGetPageRequestParameters()
        {
            var serverParameters = new Dictionary<string, string>();

            serverParameters["query"] = "test";
            serverParameters["username"] = "admin";
            serverParameters["format"] = "xml";
            serverParameters["xml"] = "true";
            serverParameters["timestamp"] = "123456789";
            serverParameters["password"] = "test";
            serverParameters["channel"] = "de";
            serverParameters["productsPerPage"] = "12";

            var expectedPageParameters = new Dictionary<string, string>();

            expectedPageParameters["keywords"] = "test";
            expectedPageParameters["productsPerPage"] = "12";
            expectedPageParameters["test"] = "value";

            var actualPageParameters = ParametersHandlerTest.ParametersConverter.GetPageRequestParams(serverParameters);

            Assert.IsTrue(expectedPageParameters.DictionaryEquals(actualPageParameters));
        }
    }
}