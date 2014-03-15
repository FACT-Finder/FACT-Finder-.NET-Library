
using Omikron.FactFinder.Core.Server;
using Omikron.FactFinder.Core;
namespace Omikron.FactFinder.Adapter
{
    public class JsonProductCampaignAdapter : Omikron.FactFinder.Json.FF68.JsonProductCampaignAdapter
    {
        public JsonProductCampaignAdapter(DataProvider dataProvider, ParametersHandler parametersHandler)
            : base(dataProvider, parametersHandler)
        { }

        /*
         * no changes in FF 6.9
         */
    }
}
