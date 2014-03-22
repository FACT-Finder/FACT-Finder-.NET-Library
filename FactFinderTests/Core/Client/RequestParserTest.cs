using System;
using System.Collections.Specialized;
using System.Web;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Moq;
using Omikron.FactFinder.Core;
using Omikron.FactFinder.Core.Client;
using Omikron.FactFinder.Util;
using Omikron.FactFinderTests.TestUtility;

namespace Omikron.FactFinderTests.Core.Client
{
    [TestClass]
    public class RequestParserTest : BaseTest
    {
        private static RequestParser RequestParser;
        private const string expectedTarget = "http://localhost/TestShop/test.cshtml";

        private HttpContextBase PreviousHttpContext;

        [ClassInitialize()]
        public static void MyClassInitialize(TestContext testContext)
        {
            log = LogManager.GetLogger(typeof(RequestParserTest));
            RequestParser = new RequestParser();
        }

        [TestInitialize]
        public override void InitializeTest()
        {
            base.InitializeTest();
            PreviousHttpContext = HttpContextFactory.Current;
            HttpContextFactory.Current = GetMockedHttpContext();
        }

        [TestCleanup]
        public void CleanupTest()
        {
            HttpContextFactory.Current = PreviousHttpContext;
        }

        private HttpContextBase GetMockedHttpContext()
        {
            var context = new Mock<HttpContextBase>();
            var request = new Mock<HttpRequestBase>();
            var response = new Mock<HttpResponseBase>();
            var session = new Mock<HttpSessionStateBase>();
            var server = new Mock<HttpServerUtilityBase>();

            var clientParameters = new NameValueCollection() { 
                { "a", "b" },
                { "c", "d" },
                { "username", "admin" },
                { "keywords", "bmx" }
            };

            request.Setup(req => req.QueryString).Returns(clientParameters);
            request.Setup(req => req.Form).Returns(new NameValueCollection());
            request.Setup(req => req.Url).Returns(new Uri(expectedTarget));

            context.Setup(ctx => ctx.Request).Returns(request.Object);
            context.Setup(ctx => ctx.Response).Returns(response.Object);
            context.Setup(ctx => ctx.Session).Returns(session.Object);
            context.Setup(ctx => ctx.Server).Returns(server.Object);
            return context.Object;
        }

        [TestMethod]
        public void TestRequestTarget()
        {
            Assert.AreEqual(expectedTarget, RequestParser.RequestTarget);
        }

        [TestMethod]
        public void TestClientRequestParameters()
        {
            var expectedParameters = new NameValueCollection() { 
                { "a", "b" },
                { "c", "d" },
                { "username", "admin" },
                { "keywords", "bmx" },
            };

            Assert.IsTrue(expectedParameters.NameValueCollectionEquals(RequestParser.ClientRequestParameters));
        }

        [TestMethod]
        public void TestRequestParameters()
        {
            var expectedParameters = new NameValueCollection() { 
                { "a", "b" },
                { "c", "d" },
                { "query", "bmx" },
                { "channel", "de" },
            };

            Assert.IsTrue(expectedParameters.NameValueCollectionEquals(RequestParser.RequestParameters));
        }
    }
}