using System.Collections.Generic;
using System.Collections.Specialized;
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
            ProductIDs.Clear();
            var idParameters = new NameValueCollection();
            foreach(var id in productIDs)
            {
                ProductIDs.Add(id);
                idParameters.Add("id", id.ToString());
            }
            DataProvider.SetParameters(idParameters);
            RecommendationsUpToDate = false;
        }

        public void AddProductID(int productID)
        {
            ProductIDs.Add(productID);
            DataProvider.AddParameter("id", productID.ToString());
            RecommendationsUpToDate = false;
        }


    }
}
