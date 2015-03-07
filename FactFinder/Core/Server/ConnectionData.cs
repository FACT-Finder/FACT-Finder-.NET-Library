using System;
using System.Collections.Specialized;
using System.Net;

namespace Omikron.FactFinder.Core.Server
{
    public class ConnectionData
    {
        #region Request data
        public WebHeaderCollection HttpHeaderFields { get; private set; }
        public RequestType Action { get; set; }
        public NameValueCollection Parameters { get; private set; }
        #endregion

        #region Response data
        public Response Response { get; private set; }
        public Uri PreviousUrl { get; private set; }
        #endregion

        public ConnectionData(NameValueCollection parameters = null)
        {
            Parameters = parameters ?? new NameValueCollection();
            HttpHeaderFields = new WebHeaderCollection();
            SetNullResponse();
        }

        public void SetResponse(Response response, Uri url)
        {
            Response = response;
            PreviousUrl = url;
        }

        public void SetNullResponse()
        {
            Response = new NullResponse();
            PreviousUrl = null;
        }
    }
}
