using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using log4net;
using Omikron.FactFinder.Util;

namespace Omikron.FactFinder.Core.Client
{
    public class UrlBuilder
    {
        private static ILog log;

        private RequestParser RequestParser;
        private ParametersConverter ParametersConverter;

        static UrlBuilder()
        {
            log = LogManager.GetLogger(typeof(UrlBuilder));
        }

        public UrlBuilder(RequestParser requestParser)
        {
            log.Debug("Initialize new UrlBuilder.");

            RequestParser = requestParser;
            ParametersConverter = new ParametersConverter();
        }

        public UrlBuilder()
        {
            // TODO: Complete member initialization
        }

        public Uri GenerateUrl(NameValueCollection parameters, string linkTarget = null)
        {
            if (String.IsNullOrWhiteSpace(linkTarget))
            {
                linkTarget = RequestParser.RequestTarget;
            }

            parameters = ParametersConverter.ServerToClientRequestParameters(parameters);

            return new Uri(String.Format("{0}?{1}", linkTarget, parameters.ToUriQueryString()));
        }
    }
}
