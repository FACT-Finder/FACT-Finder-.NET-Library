using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace Omikron.FactFinder
{
    public class HttpDataProvider : DataProvider
    {
        public string RequestTypeString
        {
            get
            {
                switch (Type)
                {
                    case RequestType.Search:
                        return "Search.ff";
                    case RequestType.Suggest:
                        return "Suggest.ff";
                    case RequestType.ShoppingCartInformationCollector:
                        return "Scic.ff";
                    case RequestType.TagCloud:
                        return "WhatsHot.ff";
                    case RequestType.Recommendation:
                        return "Recommender.ff";
                    case RequestType.ProductCampaign:
                        return "ProductCampaign.ff";
                    case RequestType.SimilarRecords:
                        return "SimilarRecords.ff";
                    case RequestType.Compare:
                        return "Compare.ff";
                    case RequestType.Import:
                        return "Import.ff";
                    default:
                        return "";
                }
            }
        }

        protected UrlBuilder UrlBuilder;

        public HttpDataProvider(IConfiguration configuration) : base(configuration)
        {
            UrlBuilder = new UrlBuilder(configuration, new ParametersHandler(configuration), new UnixClock());
        }

        private string _data;
        private bool _dataUpToDate = false;
        public string Data
        {
            get
            {
                if (!_dataUpToDate || _data == null)
                    _data = GetData();
                return _data; 
            }
        }



        public override string GetData()
        {
            if (Type == null)
                throw new Exception("Request type not set. Request could not be sent out.");

            Uri url = GetAuthenticationUrl();

            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url.ToString());
            webRequest.KeepAlive = false;
            webRequest.Method = "GET";

            if (Configuration.Language != "")
            {
                webRequest.Headers.Add("Accept-Language", Configuration.Language);
            }

            webRequest.ContentType = "text/plain";

            HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();

            StreamReader responseStream = new StreamReader(webResponse.GetResponseStream(), Encoding.Unicode);

            string response = responseStream.ReadToEnd();

            responseStream.Close();
            webResponse.Close();

            return response;
        }

        private Uri GetAuthenticationUrl()
        {
            switch (Configuration.AuthenticationType)
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

        public void SetParameters(IDictionary<string, string> parameters)
        {
            UrlBuilder.SetParameters(parameters);
        }

        public void ResetParameters(IDictionary<string, string> parameters)
        {
            UrlBuilder.ResetParameters(parameters);
        }

        public void SetParameter(KeyValuePair<string, string> parameter)
        {
            UrlBuilder.SetParameter(parameter.Key, parameter.Value);
        }

        public void SetParameter(string name, string value)
        {
            UrlBuilder.SetParameter(name, value);
        }

        public void UnsetParameter(string name)
        {
            UrlBuilder.UnsetParameter(name);
        }
    }
}
