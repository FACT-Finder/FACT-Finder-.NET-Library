using System;
using System.Collections.Generic;
using log4net;
using Omikron.FactFinder.Core.Server;
using Omikron.FactFinder.Data;

namespace Omikron.FactFinder.Adapter
{
    public class TagCloud : AbstractAdapter
    {
        private IList<TagQuery> _tagCloud;
        public IList<TagQuery> TagCloudData
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

        public TagCloud(Request request, Core.Client.UrlBuilder urlBuilder)
            : base(request, urlBuilder)
        {
            log.Debug("Initialize new TagCloud adapter.");

            Request.Action = RequestType.TagCloud;
            Parameters["format"] = "json";
            Parameters["do"] = "getTagCloud";

            UseJsonResponseContentProcessor();
        }

        protected IList<TagQuery> CreateTagCloud()
        {
            dynamic jsonData = ResponseContent;

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
