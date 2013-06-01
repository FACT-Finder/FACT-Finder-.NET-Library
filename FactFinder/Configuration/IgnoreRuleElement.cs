using System.Configuration;

namespace Omikron.FactFinder.Configuration
{
    public class IgnoreRuleElement : ConfigurationElement
    {
        #region Static Fields

        private static ConfigurationProperty _name;

        private static ConfigurationPropertyCollection _properties;

        #endregion

        #region Properties

        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string)base[_name]; }
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get { return _properties; }
        }

        #endregion

        static IgnoreRuleElement()
        {
            _name = new ConfigurationProperty(
                "name",
                typeof(string),
                null,
                ConfigurationPropertyOptions.IsRequired
            );

            _properties = new ConfigurationPropertyCollection()
            {
                _name,
            };
        }
    }
}
