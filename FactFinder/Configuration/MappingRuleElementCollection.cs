using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Omikron.FactFinder.Configuration
{
    [ConfigurationCollection(typeof(MappingRuleElement),
       CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
    public class MappingRuleElementCollection : ConfigurationElementCollection
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

        public MappingRuleElementCollection()
        {
            _properties = new ConfigurationPropertyCollection();
        }

        #region Indexers

        public MappingRuleElement this[int index]
        {
            get { return (MappingRuleElement)base.BaseGet(index); }
            set
            {
                if (base.BaseGet(index) != null)
                {
                    base.BaseRemoveAt(index);
                }
                base.BaseAdd(index, value);
            }
        }

        new public MappingRuleElement this[string name]
        {
            get { return (MappingRuleElement)base.BaseGet(name); }
        }

        #endregion

        #region Overrides

        protected override ConfigurationElement CreateNewElement()
        {
            return new MappingRuleElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as MappingRuleElement).Name;
        }

        #endregion

        public IDictionary<string, string> ToDictionary()
        {
            var result = new Dictionary<string, string>();

            foreach (var rawRule in this)
            {
                var rule = rawRule as MappingRuleElement;
                result[rule.Name] = rule.MapTo;
            }

            return result;
        }
    }
}
