using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.Script.Serialization;
using log4net;
using Omikron.FactFinder.Core;
using Omikron.FactFinder.Core.Server;
using Omikron.FactFinder.Data;
using Omikron.FactFinder.Util.Json;

namespace Omikron.FactFinder.Json.FF67
{
    public class JsonRecommendationAdapter : Omikron.FactFinder.Json.FF66.JsonRecommendationAdapter
    {
        private static ILog log;

        static JsonRecommendationAdapter()
        {
            log = LogManager.GetLogger(typeof(JsonRecommendationAdapter));
        }

        public JsonRecommendationAdapter(DataProvider dataProvider, ParametersConverter parametersConverter, Omikron.FactFinder.Core.Client.UrlBuilder urlBuilder)
            : base(dataProvider, parametersConverter, urlBuilder)
        {
            DataProvider.Type = RequestType.Recommendation;
            DataProvider.SetParameter("format", "json");
            DataProvider.SetParameter("do", "getRecommendation");
        }

        protected override ResultRecords CreateRecommendations()
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
