using System.Collections.Generic;
using Omikron.FactFinder.Default;
using Omikron.FactFinder.Data;
using System.Web.Script.Serialization;

namespace Omikron.FactFinder.Json.FF65
{
    public class JsonRecommendationAdapter : RecommendationAdapter
    {
        public JsonRecommendationAdapter(DataProvider dataProvider)
            : base(dataProvider)
        {
            DataProvider.Type = RequestType.Recommendation;
            DataProvider.SetParameter("format", "json");
            DataProvider.SetParameter("do", "getRecommendation");
        }

        protected override Result CreateRecommendations()
        {
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            var jsonData = (object[])jsonSerializer.DeserializeObject(Data);

            int count = jsonData.Length;
            var records = new List<Record>();

            int position = 0;

            foreach (var rawRecord in jsonData)
            {
                var record = (Dictionary<string, object>)rawRecord;

                if (IDsOnly)
                {
                    string stringID = (string)record["id"];
                    Record ffRecord = new Record(stringID);
                    records.Add(ffRecord);
                    continue;
                }

                records.Add(new Record(record["id"].ToString(), 100, position, position, (Dictionary<string,object>)record["record"]));

                position++;
            }            

            return new Result(records, count);
        }
    }
}
