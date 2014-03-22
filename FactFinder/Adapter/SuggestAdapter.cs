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
    public class SuggestAdapter : AbstractAdapter
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

        private dynamic _jsonData;
        protected dynamic JsonData
        {
            get
            {
                if (_jsonData == null)
                {
                    var jsonSerializer = new JavaScriptSerializer();
                    jsonSerializer.RegisterConverters(new[] { new DynamicJsonConverter() });
                    _jsonData = jsonSerializer.Deserialize(base.Data, typeof(object));
                }
                return _jsonData;
            }
        }

        private static ILog log;

        static SuggestAdapter()
        {
            log = LogManager.GetLogger(typeof(SuggestAdapter));
        }

        public SuggestAdapter(DataProvider dataProvider, ParametersConverter parametersConverter, Omikron.FactFinder.Core.Client.UrlBuilder urlBuilder)
            : base(dataProvider, parametersConverter, urlBuilder)
        {
            log.Debug("Initialize new SuggestAdapter.");
            DataProvider.Type = RequestType.Suggest;
            DataProvider.SetParameter("format", "json");
        }

        protected IList<SuggestQuery> CreateSuggestions()
        {
            var suggestions = new List<SuggestQuery>();

            foreach (var suggestData in JsonData)
            {
                string query = (string)suggestData.name;
                suggestions.Add(new SuggestQuery(
                    query,
                    ConvertServerQueryToClientUrl((string)suggestData.searchParams),
                    (int)suggestData.hitCount,
                    (string)suggestData.type,
                    new Uri((string)suggestData.image, UriKind.RelativeOrAbsolute),
                    (string)suggestData.refKey
                ));
            }

            return suggestions;
        }

        protected string CreateRawSuggestions()
        {
            return Data;
        }
    }
}
