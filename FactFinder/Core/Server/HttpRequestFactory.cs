using System.Collections.Specialized;
using log4net;
using Omikron.FactFinder.Util;

namespace Omikron.FactFinder.Core.Server
{
    public class HttpRequestFactory : IRequestFactory
    {
        private NameValueCollection RequestParameters;

        private HttpDataProvider DataProvider;

        private static ILog log;

        static HttpRequestFactory()
        {
            log = LogManager.GetLogger(typeof(HttpRequestFactory));
        }

        public HttpRequestFactory(NameValueCollection requestParameters)
        {
            var urlBuilder = new UrlBuilder(new UnixClock());

            DataProvider = new HttpDataProvider(urlBuilder);

            RequestParameters = requestParameters;
        }

        public Request GetRequest()
        {
            var connectionData = new ConnectionData(new NameValueCollection(RequestParameters));

            return new Request(connectionData, DataProvider);
        }
    }
}
