﻿
using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using log4net;
using Omikron.FactFinder.Core;
using Omikron.FactFinder.Core.Server;
using Omikron.FactFinder.Data;
using Omikron.FactFinder.Util.Json;
namespace Omikron.FactFinder.Adapter
{
    public class ProductCampaign : AbstractAdapter
    {
        protected enum CampaignType
        {
            None,
            ProductDetailPage,
            ShoppingCart
        }

        private CampaignList _campaigns;
        public CampaignList Campaigns
        {
            get
            {
                if (ProductIDs.Count == 0)
                {
                    log.Warn("Campaigns cannot be loaded without a product ID.");
                    return new CampaignList();
                }

                if (Type == CampaignType.None)
                {
                    log.Warn("Campaign type not set.");
                    return new CampaignList();
                }

                if (_campaigns == null || !CampaignsUpToDate)
                {
                    _campaigns = CreateCampaigns();
                    CampaignsUpToDate = true;
                }
                return _campaigns;
            }
        }

        private CampaignType _type;
        protected CampaignType Type
        {
            get
            {
                return _type;
            }
            set
            {
                if (_type != value)
                    CampaignsUpToDate = false;
                _type = value;
            }
        }

        protected IList<string> ProductIDs;
        protected bool CampaignsUpToDate { get; set; }

        private dynamic _jsonData;
        protected dynamic JsonData
        {
            get
            {
                if (_jsonData == null)
                {
                    var jsonSerializer = new JavaScriptSerializer();
                    jsonSerializer.RegisterConverters(new[] { new DynamicJsonConverter() });
                    _jsonData = jsonSerializer.Deserialize(base.Data, typeof(object));
                }
                return _jsonData;
            }
        }

        private static ILog log;

        static ProductCampaign()
        {
            log = LogManager.GetLogger(typeof(ProductCampaign));
        }

        public ProductCampaign(DataProvider dataProvider, ParametersConverter parametersConverter, Omikron.FactFinder.Core.Client.UrlBuilder urlBuilder)
            : base(dataProvider, parametersConverter, urlBuilder)
        {
            log.Debug("Initialize new ProductCampaignAdapter.");

            DataProvider.Type = RequestType.ProductCampaign;
            DataProvider.SetParameter("format", "json");

            ProductIDs = new List<string>();
            CampaignsUpToDate = false;
        }

        public void MakeProductDetailCampaign()
        {
            Type = CampaignType.ProductDetailPage;
            DataProvider.SetParameter("do", "getProductCampaigns");
        }

        public void MakeShoppingCartCampaign()
        {
            Type = CampaignType.ShoppingCart;
            DataProvider.SetParameter("do", "getShoppingCartCampaigns");
        }

        public virtual void SetProductIDs(string[] productIDs)
        {
            ProductIDs = productIDs;
            DataProvider.UnsetParameter("productNumber");
            foreach (var id in productIDs)
                DataProvider.AddParameter("productNumber", id);
            CampaignsUpToDate = false;
        }

        protected CampaignList CreateCampaigns()
        {
            var campaigns = new List<Campaign>();

            foreach (var campaignData in JsonData)
            {
                var campaign = CreateEmptyCampaignObject(campaignData);

                FillCampaignObject(campaign, campaignData);

                campaigns.Add(campaign);
            }

            return new CampaignList(campaigns);
        }

        protected Campaign CreateEmptyCampaignObject(dynamic campaignData)
        {
            return new Campaign(
                (string)campaignData.name,
                "",
                new Uri((string)campaignData.target.destination, UriKind.RelativeOrAbsolute)
            );
        }

        protected void FillCampaignObject(Campaign campaign, dynamic campaignData)
        {
            FillCampaignWithFeedback(campaign, campaignData);
            FillCampaignWithPushedProducts(campaign, campaignData);
        }

        protected void FillCampaignWithFeedback(Campaign campaign, dynamic campaignData)
        {
            if (campaignData.feedbackTexts.Count > 0)
            {
                var feedback = new Dictionary<string, string>();

                foreach (var feedbackData in campaignData.feedbackTexts)
                {
                    string label = feedbackData.label.ToString();
                    if (label != "")
                        feedback[label] = (string)feedbackData.text;

                    string id = feedbackData.id.ToString();
                    if (id != "")
                        feedback[id] = (string)feedbackData.text;
                }

                campaign.AddFeedback(feedback);
            }
        }

        protected void FillCampaignWithPushedProducts(Campaign campaign, dynamic campaignData)
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
