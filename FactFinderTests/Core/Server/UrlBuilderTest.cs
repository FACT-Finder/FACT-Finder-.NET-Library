using System;
using System.Collections.Specialized;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Omikron.FactFinder;
using Omikron.FactFinder.Core.Configuration;
using Omikron.FactFinder.Core;
using Omikron.FactFinder.Core.Server;
using Omikron.FactFinder.Util;
using Omikron.FactFinderTests.TestUtility ;

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
            UrlBuilder = new UrlBuilder(new ParametersConverter(), Clock);
        }

        [TestMethod]
        public void TestChannelIsSetByDefault()
        {
            Assert.IsNotNull(UrlBuilder.GetParameters()["channel"]);
        }

        [TestMethod]
        public void TestSetSingleParameter()
        {
            UrlBuilder.UnsetAllParameters(); // clear default channel parameter
            UrlBuilder.SetParameter("query", "bmx");

            var expectedParameters = new NameValueCollection()
            {
                {"query", "bmx"}
            };

            NameValueCollection actualParameters = UrlBuilder.GetParameters();
            Assert.IsTrue(expectedParameters.NameValueCollectionEquals(actualParameters));

            UrlBuilder.SetParameter("format", "xml");

            expectedParameters["format"] = "xml";

            Assert.IsTrue(expectedParameters.NameValueCollectionEquals(UrlBuilder.GetParameters()));
        }

        [TestMethod]
        public void TestSetParameters()
        {
            var expectedParameters = new NameValueCollection()
            {
                {"query", "bmx"},
                {"channel", "de"},
                {"verbose", "true"}
            };

            UrlBuilder.SetParameters(expectedParameters);

            Assert.IsTrue(expectedParameters.NameValueCollectionEquals(UrlBuilder.GetParameters()));

            var newParameters = new NameValueCollection()
            {
                {"channel", "uk"},
                {"format", "xml"}
            };

            expectedParameters["channel"] = "uk";
            expectedParameters["format"] = "xml";

            Assert.IsFalse(expectedParameters.NameValueCollectionEquals(UrlBuilder.GetParameters()));

            UrlBuilder.SetParameters(newParameters);
            Assert.IsTrue(expectedParameters.NameValueCollectionEquals(UrlBuilder.GetParameters()));
        }

        [TestMethod]
        public void TestResetParameters()
        {
            var expectedParameters = new NameValueCollection()
            {
                {"query", "bmx"},
                {"channel", "de"},
                {"verbose", "true"}
            };

            UrlBuilder.ResetParameters(expectedParameters);

            Assert.IsTrue(expectedParameters.NameValueCollectionEquals(UrlBuilder.GetParameters()));

            var newParameters = new NameValueCollection()
            {
                {"channel", "uk"},
                {"format", "xml"}
            };

            expectedParameters["channel"] = "uk";
            expectedParameters["format"] = "xml";
            expectedParameters.Remove("query");
            expectedParameters.Remove("verbose");

            Assert.IsFalse(expectedParameters.NameValueCollectionEquals(UrlBuilder.GetParameters()));

            UrlBuilder.ResetParameters(newParameters);
            Assert.IsTrue(expectedParameters.NameValueCollectionEquals(UrlBuilder.GetParameters()));
        }

        [TestMethod]
        public void TestUnsetParameter()
        {
            UrlBuilder.UnsetAllParameters(); // clear default channel parameter
            UrlBuilder.SetParameter("query", "bmx");
            UrlBuilder.SetParameter("format", "xml");

            UrlBuilder.UnsetParameter("format");

            var expectedParameters = new NameValueCollection()
            {
                {"query", "bmx"}
            };

            Assert.IsTrue(expectedParameters.NameValueCollectionEquals(UrlBuilder.GetParameters()));
        }

        [TestMethod]
        public void TestSetAction()
        {
            string expectedAction = "Test.ff";
            UrlBuilder.Action = expectedAction;

            Assert.AreEqual(expectedAction, UrlBuilder.Action);
        }

        [TestMethod]
        public void TestNonAuthenticationUrl()
        {
            UrlBuilder.Action = "Test.ff";
            UrlBuilder.SetParameter("format", "xml");

            var expectedUri = new Uri(@"http://demoshop.fact-finder.de:80/FACT-Finder/Test.ff?channel=de&format=xml");

            Uri actualUri = UrlBuilder.GetUrlWithoutAuthentication();

            Assert.IsTrue(expectedUri.EqualsWithQueryString(actualUri));
        }

        [TestMethod]
        public void TestSimpleAuthenticationUrl()
        {
            var config = ConnectionSection.GetSection();

            Clock.StubValue = 1370000000000;

            UrlBuilder.Action = "Test.ff";
            UrlBuilder.SetParameter("format", "xml");

            var expectedUri = new Uri(
                @"http://demoshop.fact-finder.de:80/FACT-Finder/Test.ff?" + 
                @"channel=de&format=xml&timestamp=" + Clock.StubValue +
                @"&username=" + config.Authentication.UserName +
                @"&password=" + config.Authentication.Password.ToMD5()
            );

            Uri actualUri = UrlBuilder.GetUrlWithSimpleAuthentication();

            Assert.IsTrue(expectedUri.EqualsWithQueryString(actualUri));
        }

        [TestMethod]
        public void TestAdvancedAuthenticationUrl()
        {
            var config = ConnectionSection.GetSection();

            Clock.StubValue = 1370000000000;

            UrlBuilder.Action = "Test.ff";
            UrlBuilder.SetParameter("format", "xml");

            string passwordParameter = (
                config.Authentication.Prefix +
                Clock.StubValue +
                config.Authentication.Password.ToMD5() +
                config.Authentication.Postfix
            ).ToMD5();

            var expectedUri = new Uri(
                @"http://demoshop.fact-finder.de:80/FACT-Finder/Test.ff?" +
                @"channel=de&format=xml&timestamp=" + Clock.StubValue +
                @"&username=" + config.Authentication.UserName +
                @"&password=" + passwordParameter
            );

            Uri actualUri = UrlBuilder.GetUrlWithAdvancedAuthentication();

            Assert.IsTrue(expectedUri.EqualsWithQueryString(actualUri));
        }

        [TestMethod]
        public void TestHttpAuthenticationUrl()
        {
            UrlBuilder.Action = "Test.ff";
            UrlBuilder.SetParameter("format", "xml");

            var expectedUri = new Uri(
                @"http://user:userpw@demoshop.fact-finder.de:80/FACT-Finder/Test.ff?channel=de&format=xml"
            );

            Uri actualUri = UrlBuilder.GetUrlWithHttpAuthentication();

            Assert.IsTrue(expectedUri.EqualsWithQueryString(actualUri));
        }
    }
}
