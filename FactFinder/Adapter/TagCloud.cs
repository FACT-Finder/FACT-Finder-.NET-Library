using System;
using System.Collections.Generic;
using System.Web.Script.Serialization;
using log4net;
using Omikron.FactFinder.Core;
using Omikron.FactFinder.Core.Server;
using Omikron.FactFinder.Data;
using Omikron.FactFinder.Util.Json;

namespace Omikron.FactFinder.Adapter
{
    public class TagCloud : AbstractAdapter
    {
        private IList<TagQuery> _tagCloud;
        public IList<TagQuery> TagCloud
        {
            get
            {
                if (_tagCloud == null)
                    _tagCloud = CreateTagCloud();
                return _tagCloud;
            }
        }

        private static ILog log;

        static TagCloud()
        {
            log = LogManager.GetLogger(typeof(TagCloud));
        }

        public TagCloud(DataProvider dataProvider, ParametersConverter parametersConverter, Omikron.FactFinder.Core.Client.UrlBuilder urlBuilder)
            : base(dataProvider, parametersConverter, urlBuilder)
        {
            log.Debug("Initialize new TagCloudAdapter.");

            DataProvider.Type = RequestType.TagCloud;
            DataProvider.SetParameter("format", "json");
            DataProvider.SetParameter("do", "getTagCloud");
        }

        protected IList<TagQuery> CreateTagCloud()
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
