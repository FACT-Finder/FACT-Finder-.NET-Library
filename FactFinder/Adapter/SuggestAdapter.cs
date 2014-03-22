using System;
using System.Collections.Generic;
using Omikron.FactFinder.Core;
using Omikron.FactFinder.Core.Server;
using Omikron.FactFinder.Data;

namespace Omikron.FactFinder.Adapter
{
    public class JsonSuggestAdapter : Omikron.FactFinder.Json.FF68.JsonSuggestAdapter
    {
        public JsonSuggestAdapter(DataProvider dataProvider, ParametersConverter parametersConverter, Omikron.FactFinder.Core.Client.UrlBuilder urlBuilder)
            : base(dataProvider, parametersConverter, urlBuilder)
        { }

        protected override IList<SuggestQuery> CreateSuggestions()
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
    }
}
