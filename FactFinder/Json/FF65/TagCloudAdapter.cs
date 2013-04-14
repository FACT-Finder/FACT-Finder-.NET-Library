using System.Collections.Generic;
using Omikron.FactFinder.Default;
using Omikron.FactFinder.Data;
using System.Web.Script.Serialization;
using Omikron.FactFinder.Json.Helper;
using log4net;

namespace Omikron.FactFinder.Json.FF65
{
    public class JsonTagCloudAdapter : TagCloudAdapter
    {
        private static ILog log;

        static JsonTagCloudAdapter()
        {
            log = LogManager.GetLogger(typeof(JsonTagCloudAdapter));
        }

        public JsonTagCloudAdapter(DataProvider dataProvider, ParametersHandler parametersHandler)
            : base(dataProvider, parametersHandler)
        {
            DataProvider.Type = RequestType.TagCloud;
            DataProvider.SetParameter("format", "json");
            DataProvider.SetParameter("do", "getTagCloud");
        }

        protected override IList<TagQuery> CreateTagCloud()
        {
            var jsonSerializer = new JavaScriptSerializer();

            jsonSerializer.RegisterConverters(new[] { new DynamicJsonConverter() });

            dynamic jsonData = jsonSerializer.Deserialize(Data, typeof(object));

            var tagCloud = new List<TagQuery>();

            foreach (var tagQuery in jsonData)
            {
                tagCloud.Add(new TagQuery(
                    (string)tagQuery.query,
                    (string)tagQuery.@params,
                    false,
                    (float)tagQuery.weight,
                    (int)tagQuery.searchCount
                ));
            }

            return tagCloud;
        }
    }
}
