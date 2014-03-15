using System.Collections.Generic;
using System.Web.Script.Serialization;
using log4net;
using Omikron.FactFinder.Core.Server;
using Omikron.FactFinder.Data;
using Omikron.FactFinder.Default;
using Omikron.FactFinder.Json.Helper;

namespace Omikron.FactFinder.Json.FF66
{
    public class JsonCompareAdapter : CompareAdapter
    {
        private static ILog log;
        
        static JsonCompareAdapter()
        {
            log = LogManager.GetLogger(typeof(JsonCompareAdapter));
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

        public JsonCompareAdapter(DataProvider dataProvider, ParametersHandler parametersHandler)
            : base(dataProvider, parametersHandler)
        {
            DataProvider.Type = RequestType.Compare;
            DataProvider.SetParameter("format", "json");
        }

        protected override IDictionary<string, bool> CreateComparableAttributes()
        {
            var attributes = new Dictionary<string, bool>();

            foreach (var attributeData in JsonData.attributes)
                attributes[attributeData.attributeName] = (bool)attributeData.different;

            return attributes;
        }

        protected override IList<Record> CreateComparedRecords()
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
