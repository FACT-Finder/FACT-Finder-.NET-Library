using System.Collections.Generic;
using log4net;
using Omikron.FactFinder.Adapter;
using Omikron.FactFinder.Data;

namespace Omikron.FactFinder.Default
{
    public class RecommendationAdapter: AbstractAdapter
    {
        private ResultRecords _recommendations;
        public ResultRecords Recommendations
        {
            get
            {
                if (ProductIDs.Count == 0)
                {
                    log.Warn("Recommendations cannot be loaded without a product ID.");
                    return new ResultRecords();
                }

                if (_recommendations == null || !RecommendationsUpToDate)
                {
                    _recommendations = CreateRecommendations();
                    RecommendationsUpToDate = true;
                }
                return _recommendations; 
            }
        }

        protected IList<string> ProductIDs;
        protected bool RecommendationsUpToDate { get; set; }

        private bool _idsOnly;
        public bool IDsOnly
        {
            protected get
            {
                return _idsOnly;
            }
            set
            {
                if (_idsOnly && !value)
                    RecommendationsUpToDate = false;
                _idsOnly = value;
                DataProvider.SetParameter("idsOnly", value ? "true" : "false");
            }
        }
        
        private int _maxResults;
        public int MaxResults
        {
            protected get
            {
                return _maxResults;
            }
            set
            {
                _maxResults = value < 1 ? 0 : value;
                if (value > 0)
                    DataProvider.SetParameter("maxResults", value.ToString());
                else
                    DataProvider.UnsetParameter("maxResults");
            }
        }

        private static ILog log;

        static RecommendationAdapter()
        {
            log = LogManager.GetLogger(typeof(RecommendationAdapter));
        }

        public RecommendationAdapter(DataProvider dataProvider, ParametersHandler parametersHandler)
            : base(dataProvider, parametersHandler)
        {
            log.Debug("Initialize new RecommendationAdapter.");
            
            ProductIDs = new List<string>();
            RecommendationsUpToDate = false;
            MaxResults = 0;
            IDsOnly = false;
        }

        public virtual void SetProductID(string productID)
        {
            ProductIDs.Clear();
            ProductIDs.Add(productID);
            DataProvider.SetParameter("id", productID);
            RecommendationsUpToDate = false;
        }
        
        protected virtual ResultRecords CreateRecommendations()
        {
            return new ResultRecords();
        }
    }
}
