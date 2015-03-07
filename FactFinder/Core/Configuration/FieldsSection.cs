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
        public string RecordId
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
        public string ProductNumber
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
        public string ProductName
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
        public string Price
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
        public string ImageUrl
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
        public string Deeplink
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
        public string MasterProductNumber
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
        public string Ean
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
        public string Brand
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
        public string Description
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
            stringRepresentationOfInstance.Append("recordId=").Append(RecordId).Append("; productNumber=").Append(ProductNumber).Append("; masterProductNumber=").Append(MasterProductNumber).Append("; ean=").Append(Ean).Append("; productName=").Append(ProductName).Append("; brand=").Append(Brand).Append("; price=").Append(Price).Append("; description=").Append(Description).Append("; imageUrl=").Append(ImageUrl).Append("; deeplink=").Append(Deeplink);
            return stringRepresentationOfInstance.ToString();
        }


        public static FieldsSection GetInstance()
        {
            return FieldSectionProvider.Instance;
        }

        private static class FieldSectionProvider
        {
            public static FieldsSection Instance = ConfigurationManager.GetSection("fields") as FieldsSection;

            static FieldSectionProvider()
            {
                if (log.IsDebugEnabled)
                {
                    log.Debug("Using field configuration: " + Instance.ToString());
                }
            }
        }
    }
}
