using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Omikron.FactFinder;

namespace FactFinderTests
{
    [TestClass]
    [DeploymentItem(@"Resources\configuration.xml", "Resources")]
    public class ParametersConverterTest
    {
        private static ParametersConverter ParametersConverter { get; set; }

        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            var configuration = new XmlConfiguration(@"Resources\configuration.xml");
            ParametersConverter = new ParametersConverter(configuration);
        }

        [TestMethod]
        public void TestGetServerRequestParameters()
        {
            var pageParameters = new Dictionary<string, string>();

            pageParameters.Add("keywords", "test");
            pageParameters.Add("username", "admin");
            pageParameters.Add("productsPerPage", "12");

            var expectedServerParameters = new Dictionary<string, string>();

            expectedServerParameters.Add("query", "test");
            expectedServerParameters.Add("productsPerPage", "12");
            expectedServerParameters.Add("channel", "de");

            var actualServerParameters = ParametersConverterTest.ParametersConverter.GetServerRequestParams(pageParameters);

            Assert.IsTrue(expectedServerParameters.DictionaryEquals(actualServerParameters));
        }
    }
}
