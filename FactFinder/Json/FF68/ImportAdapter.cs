using Omikron.FactFinder.Core;
using Omikron.FactFinder.Core.Server;
namespace Omikron.FactFinder.Json.FF68
{
    public class JsonImportAdapter : Omikron.FactFinder.Json.FF67.JsonImportAdapter
    {
        public JsonImportAdapter(DataProvider dataProvider, ParametersConverter parametersConverter, Omikron.FactFinder.Core.Client.UrlBuilder urlBuilder)
            : base(dataProvider, parametersConverter, urlBuilder)
        { }

        /*
         * no changes in FF 6.6
         */
    }
}
