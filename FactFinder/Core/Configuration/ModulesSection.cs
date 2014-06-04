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
        public bool useTracking
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
        public bool useSuggest
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
        public bool useTagcloud
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
        public bool useSimilarRecords
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
        public bool useRecommendations
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
        public bool useCampaigns
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
            stringRepresentationOfInstance.Append("useTracking=").Append(useTracking).Append("; useSuggest=").Append(useSuggest).Append("; useTagCloud=").Append(useTagcloud).Append("; useSimilarRecords=").Append(useSimilarRecords).Append("; useRecommendations=").Append(useRecommendations).Append("; useCampaigns=").Append(useCampaigns);
            return stringRepresentationOfInstance.ToString();
        }

        public static ModulesSection getInstance()
        {
            return ModulesSectionProvider.INSTANCE;
        }

        private static class ModulesSectionProvider
        {
            public static ModulesSection INSTANCE = ConfigurationManager.GetSection("modules") as ModulesSection;

            static ModulesSectionProvider(){
                if (log.IsDebugEnabled)
                {
                    log.Debug("Using module configuration: " + INSTANCE.ToString());
                }
            }
        }
    }
}
