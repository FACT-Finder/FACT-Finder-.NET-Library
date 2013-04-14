using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Omikron.FactFinder;
using log4net;

namespace Omikron.FactFinderTests
{
    [TestClass]
    public class ExtensionMethodsTest : BaseTest
    {
        [ClassInitialize]
        public static void InitializeClass(TestContext context)
        {
            log = LogManager.GetLogger(typeof(UrlBuilderTest));
        }

        [TestInitialize]
        public override void InitializeTest()
        {
            base.InitializeTest();
        }

        [TestMethod]
        public void TestToMD5()
        {
            string input = "whatasafepassword";

            string expected = "92c244f9aeae871922598416824b9a64";

            Assert.AreEqual(expected, input.ToMD5());
        }
    }
}
