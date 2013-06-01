using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using log4net;
using Omikron.FactFinder.Configuration;

namespace Omikron.FactFinder
{
    public class HttpDataProvider : DataProvider
    {
        public override RequestType Type
        {
            get
            {
                return base.Type;
            }
            set
            {
                base.Type = value;
                UrlBuilder.Action = value.ToString();
                _dataUpToDate = false;
            }
        }

        public override NameValueCollection Parameters
        {
            get
            {
                return UrlBuilder.GetParameters();
            }
        }

        protected UrlBuilder UrlBuilder;

        private static ILog log;

        static HttpDataProvider()
        {
            log = LogManager.GetLogger(typeof(HttpDataProvider));
        }

        public HttpDataProvider() 
            : base()
        {
            log.Debug("Initialize new HttpDataProvider.");
            UrlBuilder = new UrlBuilder(new ParametersHandler(), new UnixClock());
        }

        private string _data;
        private bool _dataUpToDate = false;
        public override string Data
        {
            get
            {
                if (!_dataUpToDate || _data == null)
                    _data = GetData();
                return _data; 
            }
        }

        public HttpStatusCode LastStatusCode { get; set; }

        private string GetData()
        {
            if (Type == null)
            {
                log.Error("Request type not set. Request could not be sent out.");
                return "";
            }

            Uri url = GetAuthenticationUrl(); 

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url.ToString());
            webRequest.KeepAlive = false;
            webRequest.Method = "GET";

            var config = ConnectionSection.GetSection();

            if (config.Language != "")
            {
                webRequest.Headers.Add("Accept-Language", config.Language);
            }

            log.InfoFormat("Sending request to URL: {0}", url.ToString());
            HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();

            LastStatusCode = webResponse.StatusCode;

            StreamReader responseStream = new StreamReader(webResponse.GetResponseStream(), true);

            string response = responseStream.ReadToEnd();

            responseStream.Close();
            webResponse.Close();

            return response;
        }

        private Uri GetAuthenticationUrl()
        {
            var config = ConnectionSection.GetSection();

            switch (config.Authentication.Type)
            {
                case AuthenticationType.Http:
                    return UrlBuilder.GetUrlWithHttpAuthentication();
                case AuthenticationType.Simple:
                    return UrlBuilder.GetUrlWithSimpleAuthentication();
                case AuthenticationType.Advanced:
                    return UrlBuilder.GetUrlWithAdvancedAuthentication();
                default:
                    return UrlBuilder.GetUrlWithoutAuthentication();
            }
        }

        public override void SetParameters(NameValueCollection parameters)
        {
            UrlBuilder.SetParameters(parameters);
            _dataUpToDate = false;
        }

        public override void ResetParameters(NameValueCollection parameters)
        {
            UrlBuilder.ResetParameters(parameters);
            _dataUpToDate = false;
        }

        public override void SetParameter(KeyValuePair<string, string> parameter)
        {
            UrlBuilder.SetParameter(parameter.Key, parameter.Value);
            _dataUpToDate = false;
        }

        public override void SetParameter(string name, string value)
        {
            UrlBuilder.SetParameter(name, value);
            _dataUpToDate = false;
        }

        public override void UnsetParameter(string name)
        {
            UrlBuilder.UnsetParameter(name);
            _dataUpToDate = false;
        }
    }
}
