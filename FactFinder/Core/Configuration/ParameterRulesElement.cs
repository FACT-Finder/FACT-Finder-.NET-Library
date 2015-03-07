using System.Configuration;

namespace Omikron.FactFinder.Core.Configuration
{
    public class ParameterRulesElement : ConfigurationElement
    {
        #region Static Fields

        private static ConfigurationProperty _ignoreRules;
        private static ConfigurationProperty _requireRules;
        private static ConfigurationProperty _mappingRules;

        private static ConfigurationPropertyCollection _properties;

        #endregion

        #region Properties

        [ConfigurationProperty("ignore")]
        public IgnoreRuleElementCollection IgnoreRules
        {
            get { return (IgnoreRuleElementCollection)base[_ignoreRules]; }
        }

        [ConfigurationProperty("require")]
        public RequireRuleElementCollection RequireRules
        {
            get { return (RequireRuleElementCollection)base[_requireRules]; }
        }

        [ConfigurationProperty("mapping")]
        public MappingRuleElementCollection MappingRules
        {
            get { return (MappingRuleElementCollection)base[_mappingRules]; }
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get { return _properties; }
        }

        #endregion

        static ParameterRulesElement()
        {
            _ignoreRules = new ConfigurationProperty(
                "ignore",
                typeof(IgnoreRuleElementCollection),
                null,
                ConfigurationPropertyOptions.None
            );

            _requireRules = new ConfigurationProperty(
                "require",
                typeof(RequireRuleElementCollection),
                null,
                ConfigurationPropertyOptions.None
            );

            _mappingRules = new ConfigurationProperty(
                "mapping",
                typeof(MappingRuleElementCollection),
                null,
                ConfigurationPropertyOptions.None
            );

            _properties = new ConfigurationPropertyCollection()
            {
                _ignoreRules,
                _requireRules,
                _mappingRules,
            };
        }
    }
}
