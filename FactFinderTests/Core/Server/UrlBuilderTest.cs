using System;
using System.Collections.Specialized;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Omikron.FactFinder.Core.Configuration;
using Omikron.FactFinder.Core.Server;
using Omikron.FactFinder.Util;
using Omikron.FactFinderTests.TestUtility;

namespace Omikron.FactFinderTests.Core.Server
{
    [TestClass]
    public class UrlBuilderTest : BaseTest
    {
        private UnixClockStub Clock { get; set; }
        private UrlBuilder UrlBuilder { get; set; }

        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            log = LogManager.GetLogger(typeof(UrlBuilderTest));
        }

        [TestInitialize]
        public override void InitializeTest()
        {
            base.InitializeTest();
            Clock = new UnixClockStub();
            UrlBuilder = new UrlBuilder(Clock);
        }

        [TestMethod]
        public void TestNonAuthenticationUrl()
        {
            var parameters = new NameValueCollection()
            {
                {"format", "json"}
            };

            var expectedUri = new Uri(@"http://demoshop.fact-finder.de:80/FACT-Finder/Search.ff?channel=de&format=json");

            Uri actualUri = UrlBuilder.GetUrlWithoutAuthentication(RequestType.Search, parameters);

            Assert.IsTrue(expectedUri.EqualsWithQueryString(actualUri));
        }

        [TestMethod]
        public void TestSimpleAuthenticationUrl()
        {
            var config = ConnectionSection.GetSection();

            Clock.StubValue = 1370000000000;

            var parameters = new NameValueCollection()
            {
                {"format", "json"}
            };

            var expectedUri = new Uri(
                @"http://demoshop.fact-finder.de:80/FACT-Finder/Search.ff?" + 
                @"channel=de&format=json&timestamp=" + Clock.StubValue +
                @"&username=" + config.Authentication.UserName +
                @"&password=" + config.Authentication.Password.ToMD5()
            );

            Uri actualUri = UrlBuilder.GetUrlWithSimpleAuthentication(RequestType.Search, parameters);

            Assert.IsTrue(expectedUri.EqualsWithQueryString(actualUri));
        }

        [TestMethod]
        public void TestAdvancedAuthenticationUrl()
        {
            var config = ConnectionSection.GetSection();

            Clock.StubValue = 1370000000000;

            var parameters = new NameValueCollection()
            {
                {"format", "json"}
            };

            string passwordParameter = (
                config.Authentication.Prefix +
                Clock.StubValue +
                config.Authentication.Password.ToMD5() +
                config.Authentication.Postfix
            ).ToMD5();

            var expectedUri = new Uri(
                @"http://demoshop.fact-finder.de:80/FACT-Finder/Search.ff?" +
                @"channel=de&format=json&timestamp=" + Clock.StubValue +
                @"&username=" + config.Authentication.UserName +
                @"&password=" + passwordParameter
            );

            Uri actualUri = UrlBuilder.GetUrlWithAdvancedAuthentication(RequestType.Search, parameters);

            Assert.IsTrue(expectedUri.EqualsWithQueryString(actualUri));
        }

        [TestMethod]
        public void TestHttpAuthenticationUrl()
        {
            var parameters = new NameValueCollection()
            {
                {"format", "json"}
            };

            var expectedUri = new Uri(
                @"http://user:userpw@demoshop.fact-finder.de:80/FACT-Finder/Search.ff?channel=de&format=json"
            );

            Uri actualUri = UrlBuilder.GetUrlWithHttpAuthentication(RequestType.Search, parameters);

            Assert.IsTrue(expectedUri.EqualsWithQueryString(actualUri));
        }
    }
}
