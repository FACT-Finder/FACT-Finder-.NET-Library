﻿using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;

namespace Omikron.FactFinder.Configuration
{
    public class MappingRuleElement : ConfigurationElement
    {
        #region Static Fields

        private static ConfigurationProperty _name;
        private static ConfigurationProperty _mapTo;

        private static ConfigurationPropertyCollection _properties;

        #endregion

        #region Properties

        [ConfigurationProperty("name", IsRequired = true)]
        public string Name
        {
            get { return (string)base[_name]; }
        }

        [ConfigurationProperty("mapTo", IsRequired = true)]
        public string MapTo
        {
            get { return (string)base[_mapTo]; }
        }

        protected override ConfigurationPropertyCollection Properties
        {
            get { return _properties; }
        }

        #endregion

        static MappingRuleElement()
        {
            _name = new ConfigurationProperty(
                "name",
                typeof(string),
                null,
                ConfigurationPropertyOptions.IsRequired
            );

            _mapTo = new ConfigurationProperty(
                "mapTo",
                typeof(string),
                null,
                ConfigurationPropertyOptions.IsRequired
            );

            _properties = new ConfigurationPropertyCollection()
            {
                _name,
                _mapTo,
            };
        }
    }
}