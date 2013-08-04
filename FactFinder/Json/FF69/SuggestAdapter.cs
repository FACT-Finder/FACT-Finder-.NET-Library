using System;
using System.Collections.Generic;
using Omikron.FactFinder.Data;

namespace Omikron.FactFinder.Json.FF69
{
    public class JsonSuggestAdapter : Omikron.FactFinder.Json.FF68.JsonSuggestAdapter
    {
        public JsonSuggestAdapter(DataProvider dataProvider, ParametersHandler parametersHandler)
            : base(dataProvider, parametersHandler)
        { }

        protected override IList<SuggestQuery> CreateSuggestions()
        {
            var suggestions = new List<SuggestQuery>();

            foreach (var suggestData in JsonData)
            {
                string query = (string)suggestData.name;
                suggestions.Add(new SuggestQuery(
                    query,
                    ParametersHandler.GeneratePageLink(
                        ParametersHandler.ParseParametersFromString((string)suggestData.searchParams)
                    ),
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
