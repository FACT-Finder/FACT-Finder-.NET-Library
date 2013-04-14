using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Omikron.FactFinder.Configuration
{
    public class AuthenticationElement : ConfigurationElement
    {
        #region Static Fields

        private static ConfigurationProperty _type;
        private static ConfigurationProperty _username;
        private static ConfigurationProperty _password;
        private static ConfigurationProperty _prefix;
        private static ConfigurationProperty _postfix;

        private static ConfigurationPropertyCollection _properties;

        #endregion

        #region Properties

        [ConfigurationProperty("type", IsRequired=true)]
        public AuthenticationType Type
        {
            get { return (AuthenticationType)base[_type]; }
        }

        [ConfigurationProperty("username", IsRequired = true)]
        public string UserName
        {
            get { return (string)base[_username]; }
        }

        [ConfigurationProperty("password", IsRequired = true)]
        public string Password
        {
            get { return (string)base[_password]; }
        }

        [ConfigurationProperty("advancedPrefix", IsRequired = true)]
        public string Prefix
        {
            get { return (string)base[_prefix]; }
        }

        [ConfigurationProperty("advancedPostfix", IsRequired = true)]
        public string Postfix
        {
            get { return (string)base[_postfix]; }
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get { return _properties; }
        }

        #endregion

        static AuthenticationElement()
        {
            _type = new ConfigurationProperty(
                "type",
                typeof(AuthenticationType),
                null,
                ConfigurationPropertyOptions.IsRequired
            );

            _username = new ConfigurationProperty(
                "username",
                typeof(string),
                null,
                ConfigurationPropertyOptions.IsRequired
            );

            _password = new ConfigurationProperty(
                "password",
                typeof(string),
                null,
                ConfigurationPropertyOptions.IsRequired
            );

            _prefix = new ConfigurationProperty(
                "advancedPrefix",
                typeof(string),
                null,
                ConfigurationPropertyOptions.IsRequired
            );

            _postfix = new ConfigurationProperty(
                "advancedPostfix",
                typeof(string),
                null,
                ConfigurationPropertyOptions.IsRequired
            );

            _properties = new ConfigurationPropertyCollection()
            {
                _type,
                _username,
                _password,
                _prefix,
                _postfix,
            };
        }
    }
}
