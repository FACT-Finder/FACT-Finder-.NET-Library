using log4net;
using Omikron.FactFinder.Default;

namespace Omikron.FactFinder.Json.FF65
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
