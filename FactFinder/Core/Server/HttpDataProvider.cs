using System;
using System.Collections.Specialized;
using System.IO;
using System.Net;
using log4net;
using Omikron.FactFinder.Core.Configuration;

namespace Omikron.FactFinder.Core.Server
{
    public class HttpDataProvider : DataProvider
    {
        protected UrlBuilder UrlBuilder;

        private static ILog log;

        static HttpDataProvider()
        {
            log = LogManager.GetLogger(typeof(HttpDataProvider));
        }

        public HttpDataProvider(UrlBuilder urlBuilder) 
            : base()
        {
            log.Debug("Initialize new HttpDataProvider.");
            UrlBuilder = urlBuilder;
        }

        override public void LoadResponse(int id)
        {
            if (!ConnectionData.ContainsKey(id))
                throw new ArgumentException(String.Format("Tried to get response for invalid ID {0}.", id));

            if (!HasUrlChanged(id))
                return;

            var connectionData = ConnectionData[id];
            
            if (connectionData.Action == null)
            {
                log.Error("Request type not set. Request could not be sent out.");
                connectionData.SetNullResponse();
                return;
            }

            Uri url = UrlBuilder.GetUrlWithAuthentication(
                connectionData.Action,
                connectionData.Parameters
            );

            var response = RetrieveResponse(connectionData, url);

            connectionData.SetResponse(response, url);

            LogResult(response);
        }

        private Response RetrieveResponse(ConnectionData connectionData, Uri url)
        {
            HttpWebRequest webRequest = (HttpWebRequest)WebRequest.Create(url);
            webRequest.KeepAlive = false;
            webRequest.Method = "GET";

            webRequest.Headers.Add(connectionData.HttpHeaderFields);

            var config = ConnectionSection.GetSection();

            if (config.Language != "")
            {
                webRequest.Headers.Add(HttpRequestHeader.AcceptLanguage, config.Language);
            }

            log.InfoFormat("Sending request to URL: {0}", url);
            HttpWebResponse webResponse = (HttpWebResponse)webRequest.GetResponse();

            var statusCode = webResponse.StatusCode;

            StreamReader responseStream = new StreamReader(webResponse.GetResponseStream(), true);

            string responseText = responseStream.ReadToEnd();

            responseStream.Close();
            webResponse.Close();

            return new Response(
                responseText,
                statusCode
            );
        }

        private void LogResult(Response response)
        {
            var httpCode = (int)response.StatusCode;
            if (httpCode >= 400)
                log.ErrorFormat("Connection failed. HTTP code: {0}", httpCode);
            else if (httpCode / 100 == 2)
                log.Info("Request successful!");
            else
                log.InfoFormat("Got HTTP code {0}", httpCode);
        }

        private bool HasUrlChanged(int id)
        {
            var connectionData = ConnectionData[id];

            if (connectionData.Response is NullResponse)
                return true;

            var url = UrlBuilder.GetUrlWithoutAuthentication(
                connectionData.Action,
                PrepareParameters(connectionData)
            );

            // TODO: Should we apply URL normalisation here?
            // This code could be used to do that: http://stackoverflow.com/a/14977826/1633117
            return url != connectionData.PreviousUrl;
        }

        private NameValueCollection PrepareParameters(ConnectionData connectionData)
        {
            var parameters = connectionData.Parameters;

            // TODO: Should we introduce a debug flag into the app.config?
            // If so, we'd set the parameters['verbose'] = true; here

            return parameters;
        }
    }
}