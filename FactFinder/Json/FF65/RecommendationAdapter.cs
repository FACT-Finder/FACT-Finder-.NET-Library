using System.Collections.Generic;
using Omikron.FactFinder.Default;
using Omikron.FactFinder.Data;
using System.Web.Script.Serialization;
using Omikron.FactFinder.Json.Helper;
using log4net;

namespace Omikron.FactFinder.Json.FF65
{
    public class JsonRecommendationAdapter : RecommendationAdapter
    {
        private static ILog log;

        static JsonRecommendationAdapter()
        {
            log = LogManager.GetLogger(typeof(JsonRecommendationAdapter));
        }

        public JsonRecommendationAdapter(DataProvider dataProvider, ParametersHandler parametersHandler)
            : base(dataProvider, parametersHandler)
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
    }
}
