using System;
using Omikron.FactFinder.Core.Server;
using Omikron.FactFinder.Default;
using Omikron.FactFinder.Core;

namespace Omikron.FactFinder.Json.FF66
{
    public class JsonRecommendationAdapter : RecommendationAdapter
    {
        public JsonRecommendationAdapter(DataProvider dataProvider, ParametersHandler parametersHandler)
            : base(dataProvider, parametersHandler)
        { }

        protected override Data.ResultRecords CreateRecommendations()
        {
            throw new NotSupportedException("FF 6.6 does not provide a JSON API for its recommendation engine.");
        }
    }
}
