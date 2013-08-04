using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.Script.Serialization;
using log4net;
using Omikron.FactFinder.Data;
using Omikron.FactFinder.Json.Helper;

namespace Omikron.FactFinder.Json.FF68
{
    public class JsonProductCampaignAdapter : Omikron.FactFinder.Json.FF67.JsonProductCampaignAdapter
    {
        private static ILog log;

        static JsonProductCampaignAdapter()
        {
            log = LogManager.GetLogger(typeof(JsonProductCampaignAdapter));
        }

        public JsonProductCampaignAdapter(DataProvider dataProvider, ParametersHandler parametersHandler)
            : base(dataProvider, parametersHandler)
        {
            DataProvider.Type = RequestType.ProductCampaign;
            DataProvider.SetParameter("format", "json");
        }

        // FF 6.8 removed the advisor from product detail and shopping cart campaigns
        protected override void FillCampaignObject(Campaign campaign, dynamic campaignData)
        {
            FillCampaignWithFeedback(campaign, campaignData);
            FillCampaignWithPushedProducts(campaign, campaignData);
        }

        protected override void FillCampaignWithPushedProducts(Campaign campaign, dynamic campaignData)
        {
            if (campaignData.pushedProductsRecords.Count > 0)
            {
                var pushedProducts = new List<Record>();

                foreach (var recordData in campaignData.pushedProductsRecords)
                {
                    var record = new Record((string)recordData.id);
                    record.SetFieldValues(recordData.record.AsDictionary());
                    pushedProducts.Add(record);
                }

                campaign.AddPushedProducts(pushedProducts);
            }
        }
    }
}
