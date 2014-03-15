using System.Collections.Generic;
using Omikron.FactFinder.Core.Server;
using Omikron.FactFinder.Data;
using Omikron.FactFinder.Core;
namespace Omikron.FactFinder.Json.FF68
{
    public class JsonSearchAdapter : Omikron.FactFinder.Json.FF67.JsonSearchAdapter
    {
        public JsonSearchAdapter(DataProvider dataProvider, ParametersHandler parametersHandler)
            : base(dataProvider, parametersHandler)
        { }

        protected override CampaignList CreateCampaigns()
        {
            var campaigns = new List<Campaign>();

            if (JsonData.searchResult.ContainsKey("campaigns"))
            {
                foreach (var campaignData in JsonData.searchResult.campaigns)
                {
                    var campaign = CreateEmptyCampaignObject(campaignData);

                    FillCampaignObject(campaign, campaignData);

                    campaigns.Add(campaign);
                }
            }

            return new CampaignList(campaigns);
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
