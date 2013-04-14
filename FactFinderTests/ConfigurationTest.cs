using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using log4net;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using Omikron.FactFinder.Configuration;

namespace Omikron.FactFinderTests
{
    [TestClass]
    public class ConfigurationTest : BaseTest
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
        public void TestConnectionSection()
        {
            var section = ConnectionSection.GetSection();

            if (section != null)
            {
                Assert.AreEqual(ConnectionProtocol.Http, section.Protocol);
                Assert.AreEqual(@"demoshop.fact-finder.de", section.ServerAddress);
                Assert.AreEqual(80, section.Port);
                Assert.AreEqual(@"FACT-Finder", section.Context);
                Assert.AreEqual(@"de", section.Channel);
                Assert.AreEqual(@"de", section.Language);

                Assert.AreEqual(@"user", section.Authentication.UserName);
                Assert.AreEqual(@"userpw", section.Authentication.Password);
                Assert.AreEqual(AuthenticationType.Simple, section.Authentication.Type);
                Assert.AreEqual(@"FACT-FINDER", section.Authentication.Prefix);
                Assert.AreEqual(@"FACT-FINDER", section.Authentication.Postfix);
            }
        }

        [TestMethod]
        public void TestParametersSection()
        {
            var section = ParametersSection.GetSection();

            if (section != null)
            {
                #region Test server settings

                var expectedServerIgnore = new List<string>()
                {
                    "sid",
                    "password",
                    "username",
                    "timestamp",
                };

                Assert.AreEqual(expectedServerIgnore.Count, section.Server.IgnoreRules.Count);
                foreach (var rawRule in section.Server.IgnoreRules)
                {
                    var rule = rawRule as IgnoreRuleElement;
                    Assert.IsTrue(expectedServerIgnore.Contains(rule.Name));
                }

                var expectedServerRequire = new Dictionary<string, string>();

                Assert.AreEqual(expectedServerRequire.Count, section.Server.RequireRules.Count);
                foreach (var rawRule in section.Server.RequireRules)
                {
                    var rule = rawRule as RequireRuleElement;
                    Assert.IsTrue(expectedServerRequire[rule.Name] == rule.Default);
                }

                var expectedServerMappings = new Dictionary<string, string>()
                {
                    {"keywords", "query"},
                };

                Assert.AreEqual(expectedServerMappings.Count, section.Server.MappingRules.Count);
                foreach (var rawRule in section.Server.MappingRules)
                {
                    var rule = rawRule as MappingRuleElement;
                    Assert.IsTrue(expectedServerMappings[rule.Name] == rule.MapTo);
                }

                #endregion

                #region Test client settings

                var expectedClientIgnore = new List<string>()
                {
                    "xml",
                    "format",
                    "channel",
                    "password",
                    "username",
                    "timestamp",
                };

                Assert.AreEqual(expectedClientIgnore.Count, section.Client.IgnoreRules.Count);
                foreach (var rawRule in section.Client.IgnoreRules)
                {
                    var rule = rawRule as IgnoreRuleElement;
                    Assert.IsTrue(expectedClientIgnore.Contains(rule.Name));
                }

                var expectedClientRequire = new Dictionary<string, string>()
                {
                    {"test", "value"},
                };

                Assert.AreEqual(expectedClientRequire.Count, section.Client.RequireRules.Count);
                foreach (var rawRule in section.Client.RequireRules)
                {
                    var rule = rawRule as RequireRuleElement;
                    Assert.IsTrue(expectedClientRequire[rule.Name] == rule.Default);
                }

                var expectedClientMappings = new Dictionary<string, string>()
                {
                    {"query", "keywords"},
                };

                Assert.AreEqual(expectedClientMappings.Count, section.Client.MappingRules.Count);
                foreach (var rawRule in section.Client.MappingRules)
                {
                    var rule = rawRule as MappingRuleElement;
                    Assert.IsTrue(expectedClientMappings[rule.Name] == rule.MapTo);
                }

                #endregion
            }
        }
    }
}
