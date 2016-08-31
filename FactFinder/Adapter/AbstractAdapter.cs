using System;
using System.Collections.Specialized;
using System.Web.Script.Serialization;
using log4net;
using Omikron.FactFinder.Core.Client;
using Omikron.FactFinder.Core.Server;
using Omikron.FactFinder.Util;
using Omikron.FactFinder.Util.Json;

namespace Omikron.FactFinder.Adapter
{
    public abstract class AbstractAdapter
    {
        protected Request Request;
        protected NameValueCollection Parameters;
        protected Core.Client.UrlBuilder UrlBuilder;

        protected bool UpToDate { get; set; }

        protected delegate object processString(string str);

        private processString ProcessResponseContent;

        private Response LastResponse;
        private object _responseContent;
        protected object ResponseContent
        {
            get
            {
                var response = Request.Response;
                if (_responseContent == null ||
                    response != LastResponse)
                {
                    string content = response.Content;

                    // Add response content processors
                    _responseContent = ProcessResponseContent(content);

                    LastResponse = response;
                }

                return _responseContent;
            }
        }

        private static ILog log;

        static AbstractAdapter()
        {
            log = LogManager.GetLogger(typeof(AbstractAdapter));
        }

        public AbstractAdapter(Request request, Core.Client.UrlBuilder urlBuilder)
        {
            Request = request;
            Parameters = request.Parameters;
            UrlBuilder = urlBuilder;

            UsePassthroughResponseContentProcessor();
        }

        protected void UsePassthroughResponseContentProcessor()
        {
            UseResponseContentProcessor(x => x);
        }

        protected void UseJsonResponseContentProcessor()
        {
            UseResponseContentProcessor(content =>
            {

                var jsonSerializer = new JavaScriptSerializer();
                jsonSerializer.MaxJsonLength = Int32.MaxValue;
                jsonSerializer.RegisterConverters(new[] { new DynamicJsonConverter() });
                return jsonSerializer.Deserialize(content, typeof(object));
            });
        }

        protected void UseXmlResponseContentProcessor()
        {
            throw new NotImplementedException();
        }

        /**
         * Pass in a delegate to process the response content. This method is not
         * used within the library, but may be convenient when writing custom
         * adapters.
         */
        protected void UseResponseContentProcessor(processString callback)
        {
            ProcessResponseContent = callback;
            _responseContent = null;
        }

        protected Uri ConvertServerQueryToClientUrl(string query)
        {
            return UrlBuilder.GenerateUrl(query.ToParameters());
        }
    }
}
