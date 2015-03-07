using System.Net;

namespace Omikron.FactFinder.Core.Server
{
    public class Response
    {
        virtual public string Content { get; private set; }
        virtual public HttpStatusCode? StatusCode { get; private set; }

        public Response(string content, HttpStatusCode? statusCode)
        {
            Content = content;
            StatusCode = statusCode;
        }
    }

    public class NullResponse : Response
    {
        public NullResponse() :
            base("", null) { }
    }
}
