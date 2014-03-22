﻿using System;
using System.Collections.Generic;
using log4net;
using Omikron.FactFinder.Core;
using Omikron.FactFinder.Core.Server;
using Omikron.FactFinder.Data;

namespace Omikron.FactFinder.Json.FF68
{
    public class JsonSuggestAdapter : Omikron.FactFinder.Json.FF67.JsonSuggestAdapter
    {
        private static ILog log;

        static JsonSuggestAdapter()
        {
            log = LogManager.GetLogger(typeof(JsonSuggestAdapter));
        }

        public JsonSuggestAdapter(DataProvider dataProvider, ParametersConverter parametersConverter, Omikron.FactFinder.Core.Client.UrlBuilder urlBuilder)
            : base(dataProvider, parametersConverter, urlBuilder)
        { }

        // TODO: Utilize all new Suggest features
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
                    new Uri((string)suggestData.image, UriKind.RelativeOrAbsolute)
                ));
            }

            return suggestions;
        }
    }
}
