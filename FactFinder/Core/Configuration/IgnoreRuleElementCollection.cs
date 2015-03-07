using System.Collections.Generic;
using System.Configuration;

namespace Omikron.FactFinder.Core.Configuration
{
    [ConfigurationCollection(typeof(IgnoreRuleElement),
       CollectionType = ConfigurationElementCollectionType.AddRemoveClearMap)]
    public class IgnoreRuleElementCollection : ConfigurationElementCollection
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

        public IgnoreRuleElementCollection()
        {
            _properties = new ConfigurationPropertyCollection();
        }

        #region Indexers

        public IgnoreRuleElement this[int index]
        {
            get { return (IgnoreRuleElement)base.BaseGet(index); }
            set
            {
                if (base.BaseGet(index) != null)
                {
                    base.BaseRemoveAt(index);
                }
                base.BaseAdd(index, value);
            }
        }

        new public IgnoreRuleElement this[string name]
        {
            get { return (IgnoreRuleElement)base.BaseGet(name); }
        }

        #endregion

        #region Overrides

        protected override ConfigurationElement CreateNewElement()
        {
            return new IgnoreRuleElement();
        }

        protected override object GetElementKey(ConfigurationElement element)
        {
            return (element as IgnoreRuleElement).Name;
        }

        #endregion

        public IList<string> ToList()
        {
            var result = new List<string>();

            foreach (var rule in this)
            {
                result.Add((rule as IgnoreRuleElement).Name);
            }

            return result;
        }
    }
}
