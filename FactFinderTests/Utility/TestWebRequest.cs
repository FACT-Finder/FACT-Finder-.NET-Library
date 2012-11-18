using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;
using System.IO;

namespace Omikron.FactFinderTests.Utility
{
    // Taken from http://blog.salamandersoft.co.uk/index.php/2009/10/how-to-mock-httpwebrequest-when-unit-testing/

    class TestWebRequest : WebRequest
    {
        MemoryStream requestStream = new MemoryStream();
        MemoryStream responseStream;

        public override string Method { get; set; }
        public override string ContentType { get; set; }
        public override long ContentLength { get; set; }

        /// Initializes a new instance of <see cref="TestWebRequest"/>
        /// with the response to return.
        public TestWebRequest(string response)
        {
            responseStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(response));
        }

        /// Returns the request contents as a string.
        public string ContentAsString()
        {
            return System.Text.Encoding.UTF8.GetString(requestStream.ToArray());
        }

        public override Stream GetRequestStream()
        {
            return requestStream;
        }

        public override WebResponse GetResponse()
        {
            return new TestWebReponse(responseStream);
        }
    }
}
