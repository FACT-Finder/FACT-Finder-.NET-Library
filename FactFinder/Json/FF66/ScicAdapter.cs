using log4net;
using Omikron.FactFinder.Core.Server;
using Omikron.FactFinder.Default;
using Omikron.FactFinder.Core;

namespace Omikron.FactFinder.Json.FF66
{
    public class JsonScicAdapter : ScicAdapter
    {
        private static ILog log;

        static JsonScicAdapter()
        {
            log = LogManager.GetLogger(typeof(JsonScicAdapter));
        }

        public JsonScicAdapter(DataProvider dataProvider, ParametersHandler parametersHandler)
            : base(dataProvider, parametersHandler)
        { 
            DataProvider.Type = RequestType.ShoppingCartInformationCollector;
        }

        public override bool ApplyTracking()
        {
            return Data.Trim() == "true";
        }
    }
}
