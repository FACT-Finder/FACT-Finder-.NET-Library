using log4net;
using Omikron.FactFinder.Core;
using Omikron.FactFinder.Core.Server;
namespace Omikron.FactFinder.Json.FF67
{
    public class JsonScicAdapter : Omikron.FactFinder.Json.FF66.JsonScicAdapter
    {
        private static ILog log;

        static JsonScicAdapter()
        {
            log = LogManager.GetLogger(typeof(JsonScicAdapter));
        }

        public JsonScicAdapter(DataProvider dataProvider, ParametersConverter parametersConverter, Omikron.FactFinder.Core.Client.UrlBuilder urlBuilder)
            : base(dataProvider, parametersConverter, urlBuilder)
        { }

        /*
         * no changes in FF 6.6
         */
    }
}
