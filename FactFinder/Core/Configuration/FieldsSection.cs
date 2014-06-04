using log4net;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web.Configuration;

namespace Omikron.FactFinder.Core.Configuration
{
    public class FieldsSection : ConfigurationSection
    {
        private static ILog log;
        static FieldsSection()
        {
            log = LogManager.GetLogger(typeof(FieldsSection));
        }

        private FieldsSection() { }

        [ConfigurationProperty("recordId", DefaultValue = null, IsRequired = true, IsKey = true)]
        public string recordId
        {
            get
            {
                return (string)this["recordId"];
            }
            set
            {
                this["recordId"] = value;
            }
        }

        [ConfigurationProperty("productNumber", DefaultValue = null, IsRequired = true, IsKey = true)]
        public string productNumber
        {
            get
            {
                return (string)this["productNumber"];
            }
            set
            {
                this["productNumber"] = value;
            }
        }

        [ConfigurationProperty("productName", DefaultValue = null, IsRequired = true, IsKey = true)]
        public string productName
        {
            get
            {
                return (string)this["productName"];
            }
            set
            {
                this["productName"] = value;
            }
        }

        [ConfigurationProperty("price", DefaultValue = null, IsRequired = true, IsKey = true)]
        public string price
        {
            get
            {
                return (string)this["price"];
            }
            set
            {
                this["price"] = value;
            }
        }

        [ConfigurationProperty("imageUrl", DefaultValue = null, IsRequired = true, IsKey = true)]
        public string imageUrl
        {
            get
            {
                return (string)this["imageUrl"];
            }
            set
            {
                this["imageUrl"] = value;
            }
        }

        [ConfigurationProperty("deeplink", DefaultValue = null, IsRequired = true, IsKey = true)]
        public string deeplink
        {
            get
            {
                return (string)this["deeplink"];
            }
            set
            {
                this["deeplink"] = value;
            }
        }

        [ConfigurationProperty("masterProductNumber", DefaultValue = null, IsRequired = false, IsKey = true)]
        public string masterProductNumber
        {
            get
            {
                return (string)this["masterProductNumber"];
            }
            set
            {
                this["masterProductNumber"] = value;
            }
        }

        [ConfigurationProperty("ean", DefaultValue = null, IsRequired = false, IsKey = true)]
        public string ean
        {
            get
            {
                return (string)this["ean"];
            }
            set
            {
                this["ean"] = value;
            }
        }

        [ConfigurationProperty("brand", DefaultValue = null, IsRequired = false, IsKey = true)]
        public string brand
        {
            get
            {
                return (string)this["brand"];
            }
            set
            {
                this["brand"] = value;
            }
        }

        [ConfigurationProperty("description", DefaultValue = null, IsRequired = false, IsKey = true)]
        public string description
        {
            get
            {
                return (string)this["description"];
            }
            set
            {
                this["description"] = value;
            }
        }

        public override string ToString()
        {
            StringBuilder stringRepresentationOfInstance = new StringBuilder();
            stringRepresentationOfInstance.Append("recordId=").Append(recordId).Append("; productNumber=").Append(productNumber).Append("; masterProductNumber=").Append(masterProductNumber).Append("; ean=").Append(ean).Append("; productName=").Append(productName).Append("; brand=").Append(brand).Append("; price=").Append(price).Append("; description=").Append(description).Append("; imageUrl=").Append(imageUrl).Append("; deeplink=").Append(deeplink);
            return stringRepresentationOfInstance.ToString();
        }


        public static FieldsSection getInstance()
        {
            return FieldSectionProvider.INSTANCE;
        }

        private static class FieldSectionProvider
        {
            public static FieldsSection INSTANCE = ConfigurationManager.GetSection("fields") as FieldsSection;

            static FieldSectionProvider()
            {
                if (log.IsDebugEnabled)
                {
                    log.Debug("Using field configuration: " + INSTANCE.ToString());
                }
            }
        }
    }
}
