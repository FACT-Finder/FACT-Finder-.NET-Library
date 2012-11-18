﻿using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Omikron.FactFinder;
using System.Collections.Specialized;
using System.Security.Cryptography;
using Omikron.FactFinderTests.Utility;

namespace Omikron.FactFinderTests
{
    [TestClass]
    [DeploymentItem(@"Resources\configuration.xml", "Resources")]
    public class UrlBuilderTest
    {
        private static XmlConfiguration Configuration { get; set; }
        private UnixClockStub Clock { get; set; }
        private UrlBuilder UrlBuilder { get; set; }

        [TestInitialize()]
        public void MyTestInitialize()
        {
            Configuration = new XmlConfiguration(@"Resources\configuration.xml");
            Clock = new UnixClockStub();
            UrlBuilder = new UrlBuilder(Configuration, new ParametersHandler(Configuration), Clock);
        }

        [TestMethod]
        public void TestSetSingleParameter()
        {
            UrlBuilder.SetParameter("query", "bmx");

            var expectedParameters = new Dictionary<string, string>();
            expectedParameters["query"] = "bmx";

            IDictionary<string, string> actualParameters = UrlBuilder.GetParameters();
            Assert.IsTrue(expectedParameters.DictionaryEquals(actualParameters));

            UrlBuilder.SetParameter("format", "xml");

            expectedParameters["format"] = "xml";

            Assert.IsTrue(expectedParameters.DictionaryEquals(UrlBuilder.GetParameters()));
        }

        [TestMethod]
        public void TestSetParameters()
        {
            var expectedParameters = new Dictionary<string, string>();
            expectedParameters["query"] = "bmx";
            expectedParameters["channel"] = "de";
            expectedParameters["verbose"] = "true";

            UrlBuilder.SetParameters(expectedParameters);

            Assert.IsTrue(expectedParameters.DictionaryEquals(UrlBuilder.GetParameters()));

            var newParameters = new Dictionary<string, string>();
            newParameters["channel"] = "uk";
            newParameters["format"] = "xml";
            expectedParameters["channel"] = "uk";
            expectedParameters["format"] = "xml";

            Assert.IsFalse(expectedParameters.DictionaryEquals(UrlBuilder.GetParameters()));

            UrlBuilder.SetParameters(newParameters);
            Assert.IsTrue(expectedParameters.DictionaryEquals(UrlBuilder.GetParameters()));
        }

        [TestMethod]
        public void TestResetParameters()
        {
            var expectedParameters = new Dictionary<string, string>();
            expectedParameters["query"] = "bmx";
            expectedParameters["channel"] = "de";
            expectedParameters["verbose"] = "true";

            UrlBuilder.ResetParameters(expectedParameters);

            Assert.IsTrue(expectedParameters.DictionaryEquals(UrlBuilder.GetParameters()));

            var newParameters = new Dictionary<string, string>();
            newParameters["channel"] = "uk";
            newParameters["format"] = "xml";
            expectedParameters["channel"] = "uk";
            expectedParameters["format"] = "xml";
            expectedParameters.Remove("query");
            expectedParameters.Remove("verbose");

            Assert.IsFalse(expectedParameters.DictionaryEquals(UrlBuilder.GetParameters()));

            UrlBuilder.ResetParameters(newParameters);
            Assert.IsTrue(expectedParameters.DictionaryEquals(UrlBuilder.GetParameters()));
        }

        [TestMethod]
        public void TestUnsetParameter()
        {
            UrlBuilder.SetParameter("query", "bmx");
            UrlBuilder.SetParameter("format", "xml");

            UrlBuilder.UnsetParameter("format");

            var expectedParameters = new Dictionary<string, string>();
            expectedParameters["query"] = "bmx";

            Assert.IsTrue(expectedParameters.DictionaryEquals(UrlBuilder.GetParameters()));
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
            Clock.StubValue = 1370000000000;

            UrlBuilder.Action = "Test.ff";
            UrlBuilder.SetParameter("format", "xml");

            var expectedUri = new Uri(
                @"http://demoshop.fact-finder.de:80/FACT-Finder/Test.ff?" + 
                @"channel=de&format=xml&timestamp=" + Clock.StubValue + 
                @"&username=" + Configuration.User +
                @"&password=" + Configuration.Password.ToMD5()
            );

            Uri actualUri = UrlBuilder.GetUrlWithSimpleAuthentication();

            Assert.IsTrue(expectedUri.EqualsWithQueryString(actualUri));
        }

        [TestMethod]
        public void TestAdvancedAuthenticationUrl()
        {
            Clock.StubValue = 1370000000000;

            UrlBuilder.Action = "Test.ff";
            UrlBuilder.SetParameter("format", "xml");

            string passwordParameter = (
                Configuration.AdvancedAuthPrefix +
                Clock.StubValue +
                Configuration.Password.ToMD5() +
                Configuration.AdvancedAuthPostfix
            ).ToMD5();

            var expectedUri = new Uri(
                @"http://demoshop.fact-finder.de:80/FACT-Finder/Test.ff?" +
                @"channel=de&format=xml&timestamp=" + Clock.StubValue +
                @"&username=" + Configuration.User +
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