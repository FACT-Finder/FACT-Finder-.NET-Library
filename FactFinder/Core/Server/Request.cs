using System.Collections.Specialized;
using System.Net;

namespace Omikron.FactFinder.Core.Server
{
    public class Request
    {
        public int ID { get; private set; }

        private ConnectionData ConnectionData;
        private DataProvider DataProvider;

        public Request(ConnectionData connectionData, DataProvider dataProvider)
        {
            ID = dataProvider.Register(connectionData);

            ConnectionData = connectionData;
            DataProvider = dataProvider;
        }

        // Properties that delegate to ConnectionData
        public WebHeaderCollection HttpHeaderFields 
        { 
            get { return ConnectionData.HttpHeaderFields; } 
        }

        public NameValueCollection Parameters
        { 
            get { return ConnectionData.Parameters; } 
        }

        public RequestType Action 
        {
            set { ConnectionData.Action = value; } 
        }

        public Response Response
        {
            get
            {
                DataProvider.LoadResponse(ID);
                return ConnectionData.Response;
            }
        }
    }
}
