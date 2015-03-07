using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.Configuration;

namespace Omikron.FactFinder.Core.Configuration
{
    public class ModulesSection : ConfigurationSection
    {
        private static ILog log;
        static ModulesSection()
        {
            log = LogManager.GetLogger(typeof(ModulesSection));
        }

        private ModulesSection() { }

        [ConfigurationProperty("tracking", DefaultValue = false, IsRequired = false, IsKey = true)]
        public bool UseTracking
        {
            get
            {
                return (bool)this["tracking"];
            }
            set
            {
                this["tracking"] = value;
            }
        }
        [ConfigurationProperty("suggest", DefaultValue = false, IsRequired = false, IsKey = true)]
        public bool UseSuggest
        {
            get
            {
                return (bool)this["suggest"];
            }
            set
            {
                this["suggest"] = value;
            }
        }
        [ConfigurationProperty("tagcloud", DefaultValue = false, IsRequired = false, IsKey = true)]
        public bool UseTagcloud
        {
            get
            {
                return (bool)this["tagcloud"];
            }
            set
            {
                this["tagcloud"] = value;
            }
        }
        [ConfigurationProperty("similarrecords", DefaultValue = false, IsRequired = false, IsKey = true)]
        public bool UseSimilarRecords
        {
            get
            {
                return (bool)this["similarrecords"];
            }
            set
            {
                this["similarrecords"] = value;
            }
        }
        [ConfigurationProperty("recommendations", DefaultValue = false, IsRequired = false, IsKey = true)]
        public bool UseRecommendations
        {
            get
            {
                return (bool)this["recommendations"];
            }
            set
            {
                this["recommendations"] = value;
            }
        }
        [ConfigurationProperty("campaigns", DefaultValue = false, IsRequired = false, IsKey = true)]
        public bool UseCampaigns
        {
            get
            {
                return (bool)this["campaigns"];
            }
            set
            {
                this["campaigns"] = value;
            }
        }

        public override string ToString()
        {
            StringBuilder stringRepresentationOfInstance = new StringBuilder();
            stringRepresentationOfInstance.Append("useTracking=").Append(UseTracking).Append("; useSuggest=").Append(UseSuggest).Append("; useTagCloud=").Append(UseTagcloud).Append("; useSimilarRecords=").Append(UseSimilarRecords).Append("; useRecommendations=").Append(UseRecommendations).Append("; useCampaigns=").Append(UseCampaigns);
            return stringRepresentationOfInstance.ToString();
        }

        public static ModulesSection GetInstance()
        {
            return ModulesSectionProvider.Instance;
        }

        private static class ModulesSectionProvider
        {
            public static ModulesSection Instance = ConfigurationManager.GetSection("modules") as ModulesSection;

            static ModulesSectionProvider(){
                if (log.IsDebugEnabled)
                {
                    log.Debug("Using module configuration: " + Instance.ToString());
                }
            }
        }
    }
}
