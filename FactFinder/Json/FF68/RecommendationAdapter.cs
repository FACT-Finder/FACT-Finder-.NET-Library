using Omikron.FactFinder.Core.Server;
using Omikron.FactFinder.Core;
namespace Omikron.FactFinder.Json.FF68
{
    public class JsonRecommendationAdapter : Omikron.FactFinder.Json.FF67.JsonRecommendationAdapter
    {
        public JsonRecommendationAdapter(DataProvider dataProvider, ParametersConverter parametersConverter, Omikron.FactFinder.Core.Client.UrlBuilder urlBuilder)
            : base(dataProvider, parametersConverter, urlBuilder)
        { }

        /*
         * no changes in FF 6.8
         */
    }
}
