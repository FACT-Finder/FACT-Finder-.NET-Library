using Omikron.FactFinder.Core;
using Omikron.FactFinder.Core.Server;
namespace Omikron.FactFinder.Adapter
{
    public class JsonCompareAdapter : Omikron.FactFinder.Json.FF68.JsonCompareAdapter
    {
        public JsonCompareAdapter(DataProvider dataProvider, ParametersConverter parametersConverter, Omikron.FactFinder.Core.Client.UrlBuilder urlBuilder)
            : base(dataProvider, parametersConverter, urlBuilder)
        { }

        /*
         * no changes in FF 6.9
         */
    }
}
