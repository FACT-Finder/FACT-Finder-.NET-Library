using System;
using log4net;
using log4net.Config;
using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace Omikron.FactFinderTests
{
    [TestClass]
    public class BaseTest
    {
        [AssemblyInitialize]
        public static void SetupTests(TestContext context)
        {
            XmlConfigurator.Configure();
        }

        private TestContext _testContext;
        public TestContext TestContext
        {
            get { return _testContext; }
            set { _testContext = value; }
        }

        protected static ILog log;
        /* Use this for code that has to run before every single test across
         * the entire test project.
         */
        [TestInitialize]
        public virtual void InitializeTest()
        {
            log.DebugFormat("==== Starting test \"{0}\" ====", TestContext.TestName);
        }
    }
}
