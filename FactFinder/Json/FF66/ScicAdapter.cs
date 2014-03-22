using log4net;
using Omikron.FactFinder.Core;
using Omikron.FactFinder.Core.Server;
using Omikron.FactFinder.Default;

namespace Omikron.FactFinder.Json.FF66
{
    public class JsonScicAdapter : ScicAdapter
    {
        private static ILog log;

        static JsonScicAdapter()
        {
            log = LogManager.GetLogger(typeof(JsonScicAdapter));
        }

        public JsonScicAdapter(DataProvider dataProvider, ParametersConverter parametersConverter, Omikron.FactFinder.Core.Client.UrlBuilder urlBuilder)
            : base(dataProvider, parametersConverter, urlBuilder)
        { 
            DataProvider.Type = RequestType.ShoppingCartInformationCollector;
        }

        public override bool ApplyTracking()
        {
            return Data.Trim() == "true";
        }
    }
}
