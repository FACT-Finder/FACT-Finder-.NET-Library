using System.IO;
using System.Net;

namespace Omikron.FactFinderTests.Utility
{
    // Taken from http://blog.salamandersoft.co.uk/index.php/2009/10/how-to-mock-httpwebrequest-when-unit-testing/

    class TestWebReponse : WebResponse
    {
        Stream responseStream;

        /// Initializes a new instance of <see cref="TestWebReponse"/>
        /// with the response stream to return.
        public TestWebReponse(Stream responseStream)
        {
            this.responseStream = responseStream;
        }

        public override Stream GetResponseStream()
        {
            return responseStream;
        }
    }
}
