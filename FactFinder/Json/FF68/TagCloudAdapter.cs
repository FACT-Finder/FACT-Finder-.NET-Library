using Omikron.FactFinder.Core;
using Omikron.FactFinder.Core.Server;
namespace Omikron.FactFinder.Json.FF68
{
    public class JsonTagCloudAdapter : Omikron.FactFinder.Json.FF67.JsonTagCloudAdapter
    {
        public JsonTagCloudAdapter(DataProvider dataProvider, ParametersConverter parametersConverter, Omikron.FactFinder.Core.Client.UrlBuilder urlBuilder)
            : base(dataProvider, parametersConverter, urlBuilder)
        { }

        /*
         * no changes in FF 6.8
         */
    }
}
