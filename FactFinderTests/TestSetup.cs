using System;
using log4net.Config;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Omikron.FactFinderTests
{
    [TestClass]
    public class TestSetup
    {
        [AssemblyInitialize]
        public static void SetupTests(TestContext context)
        {
            XmlConfigurator.Configure();
        }
    }
}
