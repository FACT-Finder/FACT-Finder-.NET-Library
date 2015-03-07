using System;
using System.Collections.Specialized;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Omikron.FactFinder.Core.Client;
using Omikron.FactFinderTests.TestUtility;

namespace Omikron.FactFinderTests.Core.Client
{
    [TestClass]
    public class UrlBuilderTest : BaseTest
    {
        private static UrlBuilder UrlBuilder;

        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            log = LogManager.GetLogger(typeof(UrlBuilderTest));
            var requestParser = new RequestParser();
            UrlBuilder = new UrlBuilder(requestParser);
        }

        [TestInitialize]
        public override void InitializeTest()
        {
            base.InitializeTest();
        }

        [TestMethod]
        public void TestGenerateUrlFromRequestTarget()
        {
            var parameters = new NameValueCollection() { 
                { "format", "json" },
                { "query", "bmx bike" },
            };

            var expectedUrl = new Uri("http://localhost/TestShop/index.cshtml?keywords=bmx+bike&test=value");

            Assert.IsTrue(expectedUrl.EqualsWithQueryString(UrlBuilder.GenerateUrl(parameters)));
        }

        [TestMethod]
        public void TestGenerateUrlFromExplicitTarget()
        {
            var parameters = new NameValueCollection() { 
                { "format", "json" },
                { "query", "bmx bike" },
            };

            var expectedUrl = new Uri("http://localhost/TestShop/test.cshtml?keywords=bmx+bike&test=value");

            Assert.IsTrue(expectedUrl.EqualsWithQueryString(UrlBuilder.GenerateUrl(parameters, "http://localhost/TestShop/test.cshtml")));
        }
    }
}