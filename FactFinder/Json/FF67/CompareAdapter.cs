using Omikron.FactFinder.Core.Server;
using Omikron.FactFinder.Core;
namespace Omikron.FactFinder.Json.FF67
{
    public class JsonCompareAdapter : Omikron.FactFinder.Json.FF66.JsonCompareAdapter
    {
        public JsonCompareAdapter(DataProvider dataProvider, ParametersConverter parametersConverter, Omikron.FactFinder.Core.Client.UrlBuilder urlBuilder)
            : base(dataProvider, parametersConverter, urlBuilder)
        { }

        /*
         * no changes in FF 6.7
         */
    }
}
