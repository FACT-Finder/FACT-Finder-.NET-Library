using Omikron.FactFinder.Core.Server;
using Omikron.FactFinder.Core;
namespace Omikron.FactFinder.Adapter
{
    public class JsonCompareAdapter : Omikron.FactFinder.Json.FF68.JsonCompareAdapter
    {
        public JsonCompareAdapter(DataProvider dataProvider, ParametersHandler parametersHandler)
            : base(dataProvider, parametersHandler)
        { }

        /*
         * no changes in FF 6.9
         */
    }
}
