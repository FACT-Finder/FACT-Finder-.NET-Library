using System.Collections.Generic;
using System.Configuration;

namespace Omikron.FactFinder.Core.Configuration
{
    [ConfigurationCollection(typeof(RequireRuleElement),
       CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
    public class RequireRuleElementCollection : ConfigurationElementCollection
    {
        #region Static Fields

        private static ConfigurationPropertyCollection _properties;

        #endregion

        #region Properties

        protected override ConfigurationPropertyCollection Properties
        {
            get { return _properties; }
        }

        public override ConfigurationElementCollectionType CollectionType
        {
            get { return ConfigurationElementCollectionType.AddRemoveClearMap; }
        }

        #endregion

        public RequireRuleElementCollection()
        {
            _properties = new ConfigurationPropertyCollection();
        }

        #region Indexers

        public RequireRuleElement this[int index]
        {
            get { return (RequireRuleElement)base.BaseGet(index); }
            set
            {
                if (base.BaseGet(index) != null)
                {
                    base.BaseRemoveAt(index);
                }
                base.BaseAdd(index, value);
            }
        }

        new public RequireRuleElement this[string name]
        {
            get { return (RequireRuleElement)base.BaseGet(name); }
        }

        #endregion

        #region Overrides

        protected override ConfigurationElement CreateNewElement()
        {
            return new RequireRuleElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as RequireRuleElement).Name;
        }

        #endregion

        public IDictionary<string, string> ToDictionary()
        {
            var result = new Dictionary<string, string>();

            foreach (var rawRule in this)
            {
                var rule = rawRule as RequireRuleElement;
                result[rule.Name] = rule.Default;
            }

            return result;
        }
    }
}
