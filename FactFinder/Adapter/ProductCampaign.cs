
using System;
using System.Collections.Generic;
using log4net;
using Omikron.FactFinder.Core.Server;
using Omikron.FactFinder.Data;
using System.Web;
namespace Omikron.FactFinder.Adapter
{
    public class ProductCampaign : PersonalisedResponse
    {
        protected enum CampaignType
        {
            None,
            PageCampaigns,
            ProductDetailPage,
            ShoppingCart
        }

        private CampaignList _campaigns;
        public CampaignList Campaigns
        {
            get
            {
                if (Type == CampaignType.None)
                {
                    log.Warn("Campaign type not set.");
                    return new CampaignList();
                }

                if (Type == CampaignType.ProductDetailPage || Type == CampaignType.ShoppingCart)
                {
                    if (ProductNumbers.Count == 0)
                    {
                        log.Warn("Campaigns cannot be loaded without a product number.");
                        return new CampaignList();
                    }
                }
                else
                {
                    if (String.IsNullOrEmpty(PageId))
                    {
                        log.Warn("Page campaigns cannot be loaded without a page ID.");
                        return new CampaignList();
                    }
                }

                if (_campaigns == null || !UpToDate)
                {
                    _campaigns = CreateCampaigns();
                    UpToDate = true;
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
                    UpToDate = false;
                _type = value;
            }
        }

        protected IList<string> ProductNumbers;
        protected string PageId;

        protected dynamic JsonData { get { return ResponseContent; } }

        private static ILog log;

        static ProductCampaign()
        {
            log = LogManager.GetLogger(typeof(ProductCampaign));
        }

        public ProductCampaign(Request request, Core.Client.UrlBuilder urlBuilder)
            : base(request, urlBuilder)
        {
            log.Debug("Initialize new ProductCampaign adapter.");

            Request.Action = RequestType.ProductCampaign;
            Parameters["format"] = "json";

            UseJsonResponseContentProcessor();

            ProductNumbers = new List<string>();
            UpToDate = false;
        }

        public void MakeProductDetailCampaign()
        {
            Type = CampaignType.ProductDetailPage;
            Parameters["do"] = "getProductCampaigns";
        }

        public void MakeShoppingCartCampaign()
        {
            Type = CampaignType.ShoppingCart;
            Parameters["do"] = "getShoppingCartCampaigns";
        }

        public void MakePageCampaign()
        {
            Type = CampaignType.PageCampaigns;
            Parameters["do"] = "getPageCampaigns";
        }

        public virtual void SetProductNumbers(string[] productNumbers)
        {
            ProductNumbers = productNumbers;
            Parameters.Remove("pageId");
            Parameters.Remove("productNumber");
            foreach (var productNumber in productNumbers)
                Parameters.Add("productNumber", productNumber);
            UpToDate = false;
        }

        public virtual void SetPageId(string pageId)
        {
            PageId = pageId;
            Parameters.Remove("pageId");
            Parameters.Remove("productNumber");
            Parameters.Add("pageId", pageId);
            UpToDate = false;
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
                    bool html = (bool)feedbackData.html;
                    string text = (string)feedbackData.text;
                    if (!html)
                    {
                        text = HttpUtility.HtmlAttributeEncode(text);
                    }

                    string label = feedbackData.label.ToString();
                    if (label != "")
                        feedback[label] = text;

                    string id = feedbackData.id.ToString();
                    if (id != "")
                        feedback[id] = text;
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
