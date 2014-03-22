using Omikron.FactFinder.Core;
using Omikron.FactFinder.Core.Server;
namespace Omikron.FactFinder.Adapter
{
    public class JsonSimilarRecordsAdapter : Omikron.FactFinder.Json.FF68.JsonSimilarRecordsAdapter
    {
        public JsonSimilarRecordsAdapter(DataProvider dataProvider, ParametersConverter parametersConverter, Omikron.FactFinder.Core.Client.UrlBuilder urlBuilder)
            : base(dataProvider, parametersConverter, urlBuilder)
        { }

        /*
         * no changes in FF 6.9
         */
    }
}
