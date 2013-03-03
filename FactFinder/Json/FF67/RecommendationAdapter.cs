using System.Collections.Generic;
using Omikron.FactFinder.Default;
using Omikron.FactFinder.Data;
using System.Web.Script.Serialization;
using System;
using log4net;

namespace Omikron.FactFinder.Json.FF67
{
    public class JsonRecommendationAdapter : Omikron.FactFinder.Json.FF66.JsonRecommendationAdapter
    {
        private static ILog log;

        static JsonRecommendationAdapter()
        {
            log = LogManager.GetLogger(typeof(JsonRecommendationAdapter));
        }

        public JsonRecommendationAdapter(DataProvider dataProvider, ParametersHandler parametersHandler)
            : base(dataProvider, parametersHandler)
        { }

        public void SetProductIDs(IList<int> productIDs)
        {
            throw new NotImplementedException();
        }

        public void AddProductID(int productID)
        {
            throw new NotImplementedException();
        }


    }
}
