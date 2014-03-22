using Omikron.FactFinder.Core.Server;
using Omikron.FactFinder.Core;
namespace Omikron.FactFinder.Json.FF67
{
    public class JsonTagCloudAdapter : Omikron.FactFinder.Json.FF66.JsonTagCloudAdapter
    {
        public JsonTagCloudAdapter(DataProvider dataProvider, ParametersConverter parametersConverter, Omikron.FactFinder.Core.Client.UrlBuilder urlBuilder)
            : base(dataProvider, parametersConverter, urlBuilder)
        { }

        /*
         * no changes in FF 6.7
         */
    }
}
