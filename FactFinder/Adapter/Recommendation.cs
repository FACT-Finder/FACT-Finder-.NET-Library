using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.Script.Serialization;
using log4net;
using Omikron.FactFinder.Core;
using Omikron.FactFinder.Core.Server;
using Omikron.FactFinder.Data;
using Omikron.FactFinder.Util.Json;
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

        static Recommendation()
        {
            log = LogManager.GetLogger(typeof(Recommendation));
        }

        public Recommendation(DataProvider dataProvider, ParametersConverter parametersConverter, Omikron.FactFinder.Core.Client.UrlBuilder urlBuilder)
            : base(dataProvider, parametersConverter, urlBuilder)
        {
            log.Debug("Initialize new RecommendationAdapter.");

            DataProvider.Type = RequestType.Recommendation;
            DataProvider.SetParameter("format", "json");
            DataProvider.SetParameter("do", "getRecommendation");

            ProductIDs = new List<string>();
            RecommendationsUpToDate = false;
            MaxResults = 0;
            IDsOnly = false;
        }
        

        protected ResultRecords CreateRecommendations()
        {
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            jsonSerializer.RegisterConverters(new[] { new DynamicJsonConverter() });

            dynamic jsonData = jsonSerializer.Deserialize(Data, typeof(object));

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
            DataProvider.SetParameter("id", productID);
            RecommendationsUpToDate = false;
        }

        public void SetProductIDs(IList<string> productIDs)
        {
            ProductIDs.Clear();
            var idParameters = new NameValueCollection();
            foreach(var id in productIDs)
            {
                ProductIDs.Add(id);
                idParameters.Add("id", id.ToString());
            }
            DataProvider.SetParameters(idParameters);
            RecommendationsUpToDate = false;
        }

        public void AddProductID(string productID)
        {
            ProductIDs.Add(productID);
            DataProvider.AddParameter("id", productID.ToString());
            RecommendationsUpToDate = false;
        }
    }
}
