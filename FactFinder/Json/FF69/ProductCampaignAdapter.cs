using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Web.Script.Serialization;
using log4net;
using Omikron.FactFinder.Data;
using Omikron.FactFinder.Json.Helper;

namespace Omikron.FactFinder.Json.FF69
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
