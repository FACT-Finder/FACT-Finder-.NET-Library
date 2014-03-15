using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using log4net;
using Omikron.FactFinder.Core;
using Omikron.FactFinder.Core.Server;
using Omikron.FactFinder.Data;
using Omikron.FactFinder.Default;
using Omikron.FactFinder.Json.Helper;

namespace Omikron.FactFinder.Json.FF66
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
            DataProvider.Type = RequestType.WhatsHot;
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
                    // We append an arbitrary scheme and host, because properties like Query are not
                    // defined on relative URIs.
                    new Uri((string)tagQuery.@params, UriKind.Relative),
                    false,
                    (float)tagQuery.weight,
                    (int)tagQuery.searchCount
                ));
            }

            return tagCloud;
        }
    }
}
