using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Configuration;

namespace Omikron.FactFinder.Configuration
{
    public class ParametersSection : ConfigurationSection
    {
        #region Static Fields

        private static ConfigurationProperty _serverRules;
        private static ConfigurationProperty _clientRules;

        private static ConfigurationPropertyCollection _properties;

        #endregion

        #region Properties

        [ConfigurationProperty("server", IsRequired=true)]
        public ParameterRulesElement Server
        {
            get { return (ParameterRulesElement)base[_serverRules]; }
        }

        [ConfigurationProperty("client", IsRequired = true)]
        public ParameterRulesElement Client
        {
            get { return (ParameterRulesElement)base[_clientRules]; }
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get { return _properties; }
        }

        #endregion

        static ParametersSection()
        {
            _serverRules = new ConfigurationProperty(
                "server",
                typeof(ParameterRulesElement),
                null,
                ConfigurationPropertyOptions.IsRequired
            );

            _clientRules = new ConfigurationProperty(
                "client",
                typeof(ParameterRulesElement),
                null,
                ConfigurationPropertyOptions.IsRequired
            );

            _properties = new ConfigurationPropertyCollection()
            {
                _serverRules,
                _clientRules,
            };
        }

        // This is taken from Jon Rista's Code Project article:
        // http://www.codeproject.com/Articles/16466/Unraveling-the-Mysteries-of-NET-2-0-Configuration?msg=2006420#tipsandtricks
        // If you copy this to a new section, make sure to change the default section string,
        // as well as all references to the section type.
        #region GetSection Pattern

        private static ParametersSection m_section;

        /// <summary>
        /// Gets the configuration section using the default element name.
        /// </summary>
        /// <remarks>
        /// If an HttpContext exists, uses the WebConfigurationManager
        /// to get the configuration section from web.config.
        /// </remarks>
        public static ParametersSection GetSection()
        {
            return GetSection("parameters");
        }

        /// <summary>
        /// Gets the configuration section using the specified element name.
        /// </summary>
        /// <remarks>
        /// If an HttpContext exists, uses the WebConfigurationManager
        /// to get the configuration section from web.config.
        /// </remarks>
        public static ParametersSection GetSection(string definedName)
        {
            if (m_section == null)
            {
                string cfgFileName = ".config";
                if (HttpContext.Current == null)
                {
                    m_section = ConfigurationManager.GetSection(definedName)
                                as ParametersSection;
                }
                else
                {
                    m_section = WebConfigurationManager.GetSection(definedName)
                                as ParametersSection;
                    cfgFileName = "web.config";
                }

                if (m_section == null)
                    throw new ConfigurationErrorsException("The <" + definedName +
                      "> section is not defined in your " +
                      cfgFileName + " file!");
            }

            return m_section;
        }

        #endregion
    }
}
