using log4net;
namespace Omikron.FactFinder.Json.FF67
{
    public class JsonScicAdapter : Omikron.FactFinder.Json.FF66.JsonScicAdapter
    {
        private static ILog log;

        static JsonScicAdapter()
        {
            log = LogManager.GetLogger(typeof(JsonScicAdapter));
        }

        public JsonScicAdapter(DataProvider dataProvider, ParametersHandler parametersHandler)
            : base(dataProvider, parametersHandler)
        { }

        /*
         * no changes in FF 6.6
         */
    }
}
