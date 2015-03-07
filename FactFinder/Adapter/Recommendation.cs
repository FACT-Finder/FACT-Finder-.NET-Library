using System.Collections.Generic;
using log4net;
using Omikron.FactFinder.Core.Server;
using Omikron.FactFinder.Data;
namespace Omikron.FactFinder.Adapter
{
    public class Recommendation : AbstractAdapter
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
                Parameters["idsOnly"] = value ? "true" : "false";
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
                    Parameters["maxResults"] = value.ToString();
                else
                    Parameters.Remove("maxResults");
            }
        }

        private static ILog log;

        static Recommendation()
        {
            log = LogManager.GetLogger(typeof(Recommendation));
        }

        public Recommendation(Request request, Core.Client.UrlBuilder urlBuilder)
            : base(request, urlBuilder)
        {
            log.Debug("Initialize new Recommendation adapter.");

            Request.Action = RequestType.Recommendation;
            Parameters["format"] = "json";
            Parameters["do"] = "getRecommendation";

            UseJsonResponseContentProcessor();

            ProductIDs = new List<string>();
            RecommendationsUpToDate = false;
            MaxResults = 0;
            IDsOnly = false;
        }
        

        protected ResultRecords CreateRecommendations()
        {
            dynamic jsonData = ResponseContent;

            var records = new List<Record>();

            int position = 0;

            foreach (var recordData in jsonData)
            {
                if (IDsOnly)
                {
                    Record ffRecord = new Record((string)recordData.id);
                    records.Add(ffRecord);
                    continue;
                }

                records.Add(new Record(
                    (string)recordData.id,
                    100,
                    position,
                    position,
                    recordData.record.AsDictionary()
                ));

                position++;
            }

            return new ResultRecords(records, jsonData.Length);
        }

        public void SetProductID(string productID)
        {
            ProductIDs.Clear();
            ProductIDs.Add(productID);
            Parameters["id"] = productID;
            RecommendationsUpToDate = false;
        }

        public void SetProductIDs(IList<string> productIDs)
        {
            ProductIDs.Clear();
            foreach(var id in productIDs)
                AddProductID(id);
        }

        public void AddProductID(string productID)
        {
            ProductIDs.Add(productID);
            Parameters.Add("id", productID.ToString());
            RecommendationsUpToDate = false;
        }
    }
}
