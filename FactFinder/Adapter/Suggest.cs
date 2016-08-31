using System;
using System.Collections.Generic;
using log4net;
using Omikron.FactFinder.Core.Server;
using Omikron.FactFinder.Data;

namespace Omikron.FactFinder.Adapter
{
    public class Suggest : AbstractAdapter
    {
        private IList<SuggestQuery> _suggestions;
        public IList<SuggestQuery> Suggestions
        {
            get
            {
                if (_suggestions == null)
                    _suggestions = CreateSuggestions();
                return _suggestions;
            }
        }

        private string _rawSuggestions;
        public string RawSuggestions
        {
            get
            {
                if (_rawSuggestions == null)
                    _rawSuggestions = CreateRawSuggestions();
                return _rawSuggestions;
            }
        }

        protected dynamic JsonData { get { return ResponseContent; } }

        private static ILog log;

        static Suggest()
        {
            log = LogManager.GetLogger(typeof(Suggest));
        }

        public Suggest(Request request, Core.Client.UrlBuilder urlBuilder)
            : base(request, urlBuilder)
        {
            log.Debug("Initialize new Suggest adapter.");

            Request.Action = RequestType.Suggest;
        }

        protected IList<SuggestQuery> CreateSuggestions()
        {
            UseJsonResponseContentProcessor();

            string oldFormat = null;
            if (Parameters["format"] != null)
                oldFormat = Parameters["format"];

            Parameters["format"] = "json";

            var suggestions = new List<SuggestQuery>();

            foreach (var suggestData in JsonData)
            {
                string query = (string)suggestData.name;
                suggestions.Add(new SuggestQuery(
                    query,
                    ConvertServerQueryToClientUrl((string)suggestData.searchParams),
                    (int)suggestData.hitCount,
                    (string)suggestData.type,
                    new Uri((string)suggestData.image, UriKind.RelativeOrAbsolute)
                ));
            }

            if (oldFormat != null)
                Parameters["format"] = oldFormat;

            return suggestions;
        }


        /**
         * Get the suggestions from FACT-Finder as the string returned by the
         * server.
         *
         * The format parameter is optional. Either 'json' or 'jsonp'. Use to
         * overwrite the 'format' request parameter.
         * The callback parameter is optional. Pass in a (JavaScript) function
         * name to overwrite the 'callback' request parameter, which determines 
         * the name of the callback the response is wrapped in.
         */
        protected string CreateRawSuggestions(string format = null, string callback = null)
        {
            UsePassthroughResponseContentProcessor();

            if (format != null)
                Parameters["format"] = format;
            if (callback != null)
                Parameters["callback"] = callback;

            return (string)ResponseContent;
        }
    }
}
