using System.Collections.Generic;
using log4net;
using Omikron.FactFinder.Core.Server;
using Omikron.FactFinder.Data;
namespace Omikron.FactFinder.Adapter
{
    public class Recommendation : PersonalisedResponse
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

                if (_recommendations == null || !UpToDate)
                {
                    _recommendations = CreateRecommendations();
                    UpToDate = true;
                }
                return _recommendations; 
            }
        }

        protected IList<string> ProductIDs;
        
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
            UpToDate = false;
            MaxResults = 0;
        }
        

        protected ResultRecords CreateRecommendations()
        {
            dynamic jsonData = ResponseContent;

            var records = new List<Record>();

            int position = 0;

            foreach (var recordData in jsonData)
            {
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
            UpToDate = false;
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
            UpToDate = false;
        }
    }
}
