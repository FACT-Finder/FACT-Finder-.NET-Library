using Omikron.FactFinder.Core;
using Omikron.FactFinder.Core.Server;
namespace Omikron.FactFinder.Json.FF68
{
    public class JsonScicAdapter : Omikron.FactFinder.Json.FF67.JsonScicAdapter
    {
        public JsonScicAdapter(DataProvider dataProvider, ParametersConverter parametersConverter, Omikron.FactFinder.Core.Client.UrlBuilder urlBuilder)
            : base(dataProvider, parametersConverter, urlBuilder)
        { }

        /*
         * no changes in FF 6.6
         */
    }
}
