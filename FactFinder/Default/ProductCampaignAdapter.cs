using System.Collections.Generic;
using log4net;
using Omikron.FactFinder.Data;

namespace Omikron.FactFinder.Default
{
    public class ProductCampaignAdapter : Adapter
    {
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

        private static ILog log;

        static ProductCampaignAdapter()
        {
            log = LogManager.GetLogger(typeof(RecommendationAdapter));
        }

        public ProductCampaignAdapter(DataProvider dataProvider, ParametersHandler parametersHandler)
            : base(dataProvider, parametersHandler)
        {
            log.Debug("Initialize new ProductCampaignAdapter.");
            
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
        
        protected virtual CampaignList CreateCampaigns()
        {
            return new CampaignList();
        }

        protected enum CampaignType
        {
            None,
            ProductDetailPage,
            ShoppingCart
        }
    }
}
