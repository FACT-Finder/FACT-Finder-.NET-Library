using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.Script.Serialization;
using log4net;
using Omikron.FactFinder.Data;
using Omikron.FactFinder.Default;
using Omikron.FactFinder.Json.Helper;

namespace Omikron.FactFinder.Json.FF66
{
    public class JsonSuggestAdapter : SuggestAdapter
    {
        private static ILog log;

        static JsonSuggestAdapter()
        {
            log = LogManager.GetLogger(typeof(JsonSuggestAdapter));
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

        public JsonSuggestAdapter(DataProvider dataProvider, ParametersHandler parametersHandler)
            : base(dataProvider, parametersHandler)
        {
            DataProvider.Type = RequestType.Suggest;
            DataProvider.SetParameter("format", "json");
        }

        protected override IList<SuggestQuery> CreateSuggestions()
        {
            var suggestions = new List<SuggestQuery>();

            foreach (var suggestData in JsonData)
            {
                string query = (string)suggestData.query;
                var parameters = new NameValueCollection()
                {
                    {"query", query} 
                };

                suggestions.Add(new SuggestQuery(
                    query,
                    ParametersHandler.GeneratePageLink(parameters),
                    (int)suggestData.hitCount,
                    (string)suggestData.type,
                    new Uri((string)suggestData.imageURL, UriKind.RelativeOrAbsolute)
                ));
            }

            return suggestions;
        }
    }
}
