using System.Collections.Generic;
using System.Web.Script.Serialization;
using log4net;
using Omikron.FactFinder.Data;
using Omikron.FactFinder.Default;
using Omikron.FactFinder.Json.Helper;

namespace Omikron.FactFinder.Json.FF66
{
    public class JsonSimilarRecordsAdapter : SimilarRecordsAdapter
    {
        private static ILog log;
        
        static JsonSimilarRecordsAdapter()
        {
            log = LogManager.GetLogger(typeof(JsonSimilarRecordsAdapter));
        }

        protected dynamic JsonData
        {
            get
            {
                var jsonSerializer = new JavaScriptSerializer();
                jsonSerializer.RegisterConverters(new[] { new DynamicJsonConverter() });
                return jsonSerializer.Deserialize(base.Data, typeof(object));
            }
        }

        public JsonSimilarRecordsAdapter(DataProvider dataProvider, ParametersHandler parametersHandler)
            : base(dataProvider, parametersHandler)
        {
            DataProvider.Type = RequestType.SimilarRecords;
            DataProvider.SetParameter("format", "json");
        }

        protected override IDictionary<string, string> CreateSimilarAttributes()
        {
            var attributes = new Dictionary<string, string>();

            foreach (var attributeData in JsonData.attributes)
                attributes[attributeData.name] = (string)attributeData.value;

            return attributes;
        }

        protected override IList<Record> CreateRecords()
        {
            var records = new List<Record>();

            int position = 0;

            foreach (var recordData in JsonData.records)
            {
                if (IDsOnly)
                {
                    records.Add(new Record((string)recordData.id));
                    continue;
                }

                records.Add(new Record(
                    (string)recordData.id,
                    100,
                    position,
                    position,
                    recordData.record.AsDictionary()
                ));

                ++position;
            }

            return records;
        }
    }
}
