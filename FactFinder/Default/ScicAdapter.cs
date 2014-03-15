using System.Collections.Specialized;
using log4net;
using Omikron.FactFinder.Adapter;
using Omikron.FactFinder.Core;
using Omikron.FactFinder.Core.Server;

namespace Omikron.FactFinder.Default
{
    public class ScicAdapter : AbstractAdapter
    {
        private static ILog log;

        static ScicAdapter()
        {
            log = LogManager.GetLogger(typeof(ScicAdapter));
        }

        public ScicAdapter(DataProvider dataProvider, ParametersHandler parametersHandler)
            : base(dataProvider, parametersHandler)
        {
            log.Debug("Initialize new ScicAdapter.");
        }

        // returns success boolean
        public virtual bool ApplyTracking()
        {
            return false;
        }

        /**
         * If all needed parameters are available at the request like described in the documentation, 
         * just use this method to fetch the needed parameters and track them.
         *
         * @return boolean success
         **/
        public bool DoTrackingFromRequest(string sid)
        {
            SetupTrackingFromRequest(sid);
            return ApplyTracking();
        }

        public void SetupTrackingFromRequest(string sid)
        {
            throw new System.NotImplementedException();
        }

        /**
         * track a detail click on a product
         *
         * @param string id             id of product
         * @param string sid            session id
         * @param string query          query which led to the product
         * @param int position          position of product in the search result
         * @param int originalPosition  original position of product in the search result. 
         *                              this data is delivered by FACT-Finder
         *                              (optional - is set equal to position by default)
         * @param int page              page number where the product was clicked
         *                              (optional - is 1 by default)
         * @param float similarity      similiarity of the product
         *                              (optional - is 100.00 by default)
         * @param string title          title of product
         *                              (optional - is empty by default)
         * @param int pageSize          size of the page where the product was found
         *                              (optional - is 12 by default)
         * @param int originalPageSize  original size of the page before the user could have changed it
         *                              (optional - is set equal to page by default)
         * @return boolean              success
         **/
        public bool TrackClick(
            string id,
            string sid,
            string query,
            int position,
            int originalPosition = -1,
            int page = 1,
            float similarity = 100.0f,
            string title = "",
            int pageSize = 12,
            int originalPageSize = -1
        )
        {
            SetupClickTracking(id, sid, query, position, originalPosition, page, similarity, title, pageSize, originalPageSize);
            return ApplyTracking();
        }

        public void SetupClickTracking(
            string id,
            string sid,
            string query,
            int position,
            int originalPosition = -1,
            int page = 1,
            float similarity = 100.0f,
            string title = "",
            int pageSize = 12,
            int originalPageSize = -1)
        {
            if (originalPosition == -1)
                originalPosition = position;
            if (originalPageSize == -1)
                originalPageSize = pageSize;

            var parameters = new NameValueCollection()
            {
                {"query", query},
                {"id", id},
                {"pos", position.ToString()},
                {"origPos", originalPosition.ToString()},
                {"page", page.ToString()},
                {"simi", similarity.ToString()},
                {"sid", sid},
                {"title", title},
                {"event", "click"},
                {"pageSize", pageSize.ToString()},
                {"originalPageSize", originalPageSize.ToString()}
            };

            DataProvider.SetParameters(parameters);
        }

        /**
         * track a product which was added to the cart
         *
         * @param string id    id of product
         * @param string sid   session id
         * @param int count    number of items purchased for each product
         *                     (optional - default 1)
         * @param float price  this is the single unit price
         *                     (optional)
         * @param string userid id of user checking out
         *                      (optional - default empty string)
         * @return boolean     success
         **/
        public bool TrackCart(
            string id,
            string sid,
            int count = 1,
            float price = float.NaN,
            string userID = ""
        )
        {
            SetupCartTracking(id, sid, count, price, userID);
            return ApplyTracking();
        }

        public void SetupCartTracking(
            string id,
            string sid,
            int count = 1,
            float price = float.NaN,
            string userID = ""
        )
        {
            var parameters = new NameValueCollection()
            {
                {"id", id},
                {"sid", sid},
                {"count", count.ToString()},
                {"event", "cart"}
            };

            if (float.IsNaN(price))
                parameters["price"] = price.ToString();
            if (userID != "")
                parameters["userid"] = userID;

            DataProvider.SetParameters(parameters);
        }

        /**
         * track a product which was purchased
         *
         * @param string id     id of product
         * @param string sid    session id
         * @param int count     number of items purchased for each product 
         *                      (optional - default 1)
         * @param float price   this is the single unit price 
         *                      (optional)
         * @param string userid id of user checking out
         *                      (optional - default empty string)
         * @return boolean      success
         **/
        public bool TrackCheckout(
            string id,
            string sid,
            int count = 1,
            float price = float.NaN,
            string userID = ""
        )
        {
            SetupCheckoutTracking(id, sid, count, price, userID);
            return ApplyTracking();
        }

        public void SetupCheckoutTracking(
            string id,
            string sid,
            int count = 1,
            float price = float.NaN,
            string userID = ""
        )
        {
            var parameters = new NameValueCollection()
            {
                {"id", id},
                {"sid", sid},
                {"count", count.ToString()},
                {"event", "checkout"}
            };

            if (float.IsNaN(price))
                parameters["price"] = price.ToString();
            if (userID != "")
                parameters["userid"] = userID;

            DataProvider.SetParameters(parameters);
        }

        /**
         * track a click on a recommended product
         *
         * @param string id     id of product
         * @param string sid    session id
         * @param string mainID ID of the product for which the clicked-upon item was recommended
         * @return boolean      success
         **/
        public bool TrackRecommendationClick(
            string id,
            string sid,
            string mainID
        )
        {
            SetupRecommendationClickTracking(id, sid, mainID);
            return ApplyTracking();
        }

        public void SetupRecommendationClickTracking(
            string id,
            string sid,
            string mainID
        )
        {
            var parameters = new NameValueCollection()
            {
                {"id", id},
                {"sid", sid},
                {"mainID", mainID},
                {"event", "recommendationClick"}
            };

            DataProvider.SetParameters(parameters);
        }
    }
}
