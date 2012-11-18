using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Net;

namespace Omikron.FactFinderTests.Utility
{
    // Taken from http://blog.salamandersoft.co.uk/index.php/2009/10/how-to-mock-httpwebrequest-when-unit-testing/

    class TestWebRequestCreate : IWebRequestCreate
    {
        static private WebRequest _nextRequest;
        static object lockObject = new object();

        static private WebRequest NextRequest
        {
            get { return _nextRequest; }
            set
            {
                lock (lockObject)
                {
                    _nextRequest = value;
                }
            }
        }

        public WebRequest Create(Uri uri)
        {
            return NextRequest;
        }

        /// Utility method for creating a TestWebRequest and setting
        /// it to be the next WebRequest to use.
        /// <param name="response">The response the TestWebRequest will return.</param>
        public static TestWebRequest CreateTestRequest(string response)
        {
            TestWebRequest request = new TestWebRequest(response);
            NextRequest = request;
            return request;
        }
    }
}
