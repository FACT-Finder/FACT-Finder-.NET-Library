using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;
using Omikron.FactFinder.Configuration;
using System.Configuration;

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

        public override IDictionary<string, string> Parameters
        {
            get
            {
                return UrlBuilder.GetParameters();
            }
        }

        protected UrlBuilder UrlBuilder;

        public HttpDataProvider() 
            : base()
        {
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
                throw new Exception("Request type not set. Request could not be sent out.");

            Uri url = GetAuthenticationUrl(); 

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url.ToString());
            webRequest.KeepAlive = false;
            webRequest.Method = "GET";

            var config = ConnectionSection.GetSection();

            if (config.Language != "")
            {
                webRequest.Headers.Add("Accept-Language", config.Language);
            }          

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

        public override void SetParameters(IDictionary<string, string> parameters)
        {
            UrlBuilder.SetParameters(parameters);
            _dataUpToDate = false;
        }

        public override void ResetParameters(IDictionary<string, string> parameters)
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
