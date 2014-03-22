using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using log4net;
using Omikron.FactFinder.Util;

namespace Omikron.FactFinder.Core.Client
{
    public class RequestParser
    {
        private static ILog log;

        private ParametersConverter ParametersConverter;

        private NameValueCollection _serverRequestParameters;
        public NameValueCollection RequestParameters
        {
            get
            {
                if (_serverRequestParameters == null)
                {
                    _serverRequestParameters = ParametersConverter.ClientToServerRequestParameters(ClientRequestParameters);
                }
                return _serverRequestParameters;
            }
        }

        private NameValueCollection _clientRequestParameters;
        public NameValueCollection ClientRequestParameters
        {
            get
            {
                if (_clientRequestParameters == null)
                {
                    var request = HttpContextFactory.Current.Request;
                    _clientRequestParameters = request.QueryString;
                    _clientRequestParameters.Add(request.Form);
                }
                return _clientRequestParameters;
            }
        }

        private string _requestTarget;
        public string RequestTarget
        {
            get
            {
                if (_requestTarget == null)
                {
                    var url = HttpContextFactory.Current.Request.Url;
                    _requestTarget = String.Format("{0}://{1}{2}", url.Scheme, url.Authority, url.AbsolutePath);
                }
                return _requestTarget;
            }
        }

        static RequestParser()
        {
            log = LogManager.GetLogger(typeof(RequestParser));
        }

        public RequestParser()
        {
            log.Debug("Initialize new RequestParser.");
            ParametersConverter = new ParametersConverter();
        }
    }
}
