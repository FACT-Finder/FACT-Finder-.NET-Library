using System;
using System.Configuration;
using System.Web;
using System.Web.Configuration;
using log4net;

namespace Omikron.FactFinder.Configuration
{
    public class ConnectionSection : ConfigurationSection
    {
        #region Static Fields

        private static ConfigurationProperty _protocol;
        private static ConfigurationProperty _serverAddress;
        private static ConfigurationProperty _port;
        private static ConfigurationProperty _context;
        private static ConfigurationProperty _channel;
        private static ConfigurationProperty _authentication;
        private static ConfigurationProperty _language;
        //private static ConfigurationProperty _timeouts;

        private static ConfigurationPropertyCollection _properties;

        private static ILog log;

        #endregion

        #region Properties

        [ConfigurationProperty("protocol", IsRequired=true)]
        public ConnectionProtocol Protocol
        {
            get { return (ConnectionProtocol)base[_protocol]; }
        }

        [ConfigurationProperty("address", IsRequired = true)]
        public string ServerAddress
        {
            get { return (string)base[_serverAddress]; }
        }

        [ConfigurationProperty("port", IsRequired = true)]
        public int Port
        {
            get { return (int)base[_port]; }
        }

        [ConfigurationProperty("context", IsRequired = true)]
        public string Context
        {
            get { return (string)base[_context]; }
        }

        [ConfigurationProperty("channel", IsRequired = true)]
        public string Channel
        {
            get { return (string)base[_channel]; }
        }

        [ConfigurationProperty("language", IsRequired = true)]
        public string Language
        {
            get { return (string)base[_language]; }
        }

        [ConfigurationProperty("authentication", IsRequired = true)]
        public AuthenticationElement Authentication
        {
            get { return (AuthenticationElement)base[_authentication]; }
        }

        /*[ConfigurationProperty("timeouts", IsRequired = true)]
        public TimeoutElement Timeouts
        {
            get { return (TimeoutElement)base[_timeouts]; }
        }*/

        protected override ConfigurationPropertyCollection Properties
        {
            get { return _properties; }
        }

        #endregion

        static ConnectionSection()
        {
            log = LogManager.GetLogger(typeof(ConnectionSection));

            _protocol = new ConfigurationProperty(
                "protocol",
                typeof(ConnectionProtocol),
                null,
                ConfigurationPropertyOptions.IsRequired
            );

            _serverAddress = new ConfigurationProperty(
                "address",
                typeof(string),
                null,
                ConfigurationPropertyOptions.IsRequired
            );

            _port = new ConfigurationProperty(
                "port",
                typeof(int),
                null,
                ConfigurationPropertyOptions.IsRequired
            );

            _context = new ConfigurationProperty(
                "context",
                typeof(string),
                null,
                ConfigurationPropertyOptions.IsRequired
            );

            _channel = new ConfigurationProperty(
                "channel",
                typeof(string),
                null,
                ConfigurationPropertyOptions.IsRequired
            );

            _language = new ConfigurationProperty(
                "language",
                typeof(string),
                null,
                ConfigurationPropertyOptions.IsRequired
            );

            _authentication = new ConfigurationProperty(
                "authentication",
                typeof(AuthenticationElement),
                null,
                ConfigurationPropertyOptions.IsRequired
            );

            /*_timeouts = new ConfigurationProperty(
                "timeouts",
                typeof(TimeoutElement),
                null,
                ConfigurationPropertyOptions.IsRequired
            );*/

            _properties = new ConfigurationPropertyCollection()
            {
                _protocol,
                _serverAddress,
                _port,
                _context,
                _channel,
                _language,
                _authentication,
                //_timeouts,
            };
        }

        // This is taken from Jon Rista's Code Project article:
        // http://www.codeproject.com/Articles/16466/Unraveling-the-Mysteries-of-NET-2-0-Configuration?msg=2006420#tipsandtricks
        // If you copy this to a new section, make sure to change the default section string,
        // as well as all references to the section type.
        #region GetSection Pattern
        
        private static ConnectionSection m_section;

        /// <summary>
        /// Gets the configuration section using the default element name.
        /// </summary>
        /// <remarks>
        /// If an HttpContext exists, uses the WebConfigurationManager
        /// to get the configuration section from web.config.
        /// </remarks>
        public static ConnectionSection GetSection()
        {
            return GetSection("connection");
        }

        /// <summary>
        /// Gets the configuration section using the specified element name.
        /// </summary>
        /// <remarks>
        /// If an HttpContext exists, uses the WebConfigurationManager
        /// to get the configuration section from web.config.
        /// </remarks>
        public static ConnectionSection GetSection(string definedName)
        {
            if (m_section == null)
            {
                log.Debug(String.Format("Retrieving configuration section <{0}>", definedName));
            
                string cfgFileName = ".config";
                if (HttpContext.Current == null)
                {
                    m_section = ConfigurationManager.GetSection(definedName)
                                as ConnectionSection;
                }
                else
                {
                    m_section = WebConfigurationManager.GetSection(definedName)
                                as ConnectionSection;
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
