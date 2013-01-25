using System.Collections.Generic;
using Omikron.FactFinder.Default;
using Omikron.FactFinder.Data;
using System.Web.Script.Serialization;
using System;

namespace Omikron.FactFinder.Json.FF67
{
    public class JsonRecommendationAdapter : Omikron.FactFinder.Json.FF66.JsonRecommendationAdapter
    {
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
