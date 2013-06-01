using System.Configuration;

namespace Omikron.FactFinder.Configuration
{
    public class RequireRuleElement : ConfigurationElement
    {
        #region Static Fields

        private static ConfigurationProperty _name;
        private static ConfigurationProperty _default;

        private static ConfigurationPropertyCollection _properties;

        #endregion

        #region Properties

        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string)base[_name]; }
        }

        [ConfigurationProperty("default", IsRequired = true)]
        public string Default
        {
            get { return (string)base[_default]; }
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get { return _properties; }
        }

        #endregion

        static RequireRuleElement()
        {
            _name = new ConfigurationProperty(
                "name",
                typeof(string),
                null,
                ConfigurationPropertyOptions.IsRequired
            );

            _default = new ConfigurationProperty(
                "default",
                typeof(string),
                null,
                ConfigurationPropertyOptions.IsRequired
            );

            _properties = new ConfigurationPropertyCollection()
            {
                _name,
                _default,
            };
        }
    }
}
