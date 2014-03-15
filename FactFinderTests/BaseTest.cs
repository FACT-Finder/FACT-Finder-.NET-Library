using System;
using System.Collections.Specialized;
using System.Net;
using System.Web;
using log4net;
using log4net.Config;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Omikron.FactFinder;
using Omikron.FactFinder.Util;
using Omikron.FactFinderTests.TestUtility ;

namespace Omikron.FactFinderTests
{
    [TestClass]
    public class BaseTest
    {
        [AssemblyInitialize]
        public static void SetupTests(TestContext context)
        {
            XmlConfigurator.Configure();
            HttpContextFactory.Current = GetMockedHttpContext();
            WebRequest.RegisterPrefix("http://", new TestWebRequestCreate());
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

        private static HttpContextBase GetMockedHttpContext()
        {
            var context  = new Mock<HttpContextBase>();
            var request  = new Mock<HttpRequestBase>();
            var response = new Mock<HttpResponseBase>();
            var session  = new Mock<HttpSessionStateBase>();
            var server   = new Mock<HttpServerUtilityBase>();

            request.Setup(req => req.QueryString).Returns(new NameValueCollection());
            request.Setup(req => req.Form).Returns(new NameValueCollection());
            request.Setup(req => req.Url).Returns(new Uri("http://localhost:80/TestShop/index.cshtml"));

            context.Setup(ctx => ctx.Request).Returns(request.Object);
            context.Setup(ctx => ctx.Response).Returns(response.Object);
            context.Setup(ctx => ctx.Session).Returns(session.Object);
            context.Setup(ctx => ctx.Server).Returns(server.Object);
            return context.Object;
        }
    }
}
