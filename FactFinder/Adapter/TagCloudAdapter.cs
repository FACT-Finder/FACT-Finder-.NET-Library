using Omikron.FactFinder.Core;
using Omikron.FactFinder.Core.Server;
namespace Omikron.FactFinder.Adapter
{
    public class JsonTagCloudAdapter : Omikron.FactFinder.Json.FF68.JsonTagCloudAdapter
    {
        public JsonTagCloudAdapter(DataProvider dataProvider, ParametersConverter parametersConverter, Omikron.FactFinder.Core.Client.UrlBuilder urlBuilder)
            : base(dataProvider, parametersConverter, urlBuilder)
        {
            DataProvider.Type = RequestType.TagCloud;
        }
    }
}
