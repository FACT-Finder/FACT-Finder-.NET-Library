using System.Collections.Generic;
using Omikron.FactFinder.Default;
using Omikron.FactFinder.Data;
using System.Web.Script.Serialization;
using Omikron.FactFinder.Json.Helper;
using System;

namespace Omikron.FactFinder.Json.FF65
{
    public class JsonSuggestAdapter : SuggestAdapter
    {
        public JsonSuggestAdapter(DataProvider dataProvider, ParametersHandler parametersHandler)
            : base(dataProvider, parametersHandler)
        {
            DataProvider.Type = RequestType.Suggest;
            DataProvider.SetParameter("format", "json");
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

        protected override IList<SuggestQuery> CreateSuggestions()
        {
            var suggestions = new List<SuggestQuery>();

            foreach (var suggestData in JsonData)
            {
                string query = (string)suggestData.query;
                var parameters = new Dictionary<string, string>()
                { {"query", query} };
                suggestions.Add(new SuggestQuery(
                    query,
                    ParametersHandler.GeneratePageLink(parameters),
                    (int)suggestData.hitCount,
                    (string)suggestData.type,
                    (string)suggestData.imageUrl
                ));
            }

            return suggestions;
        }
    }
}
