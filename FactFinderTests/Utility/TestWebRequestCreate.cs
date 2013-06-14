using System;
using System.Collections.Specialized;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Web;
using Moq;

namespace Omikron.FactFinderTests.Utility
{
    // Taken from http://blog.salamandersoft.co.uk/index.php/2009/10/how-to-mock-httpwebrequest-when-unit-testing/

    class TestWebRequestCreate : IWebRequestCreate
    {
        static private WebRequest NextRequest;

        static private string PathToFiles;

        public WebRequest Create(Uri uri)
        {
            var response = new Mock<HttpWebResponse>(MockBehavior.Loose);
            response.Setup(req => req.StatusCode).Returns(HttpStatusCode.OK);
            FileStream fs = File.OpenRead(GetFileName(uri));
            response.Setup(req => req.GetResponseStream()).Returns(fs);

            var request = new Mock<HttpWebRequest>();
            request.Setup(s => s.GetResponse()).Returns(response.Object);
            request.Setup(s => s.Headers).Returns(new WebHeaderCollection());
            NextRequest = request.Object;
            return request.Object;
        }

        private string GetFileName(Uri uri)
        {
            var sb = new StringBuilder(PathToFiles);
            sb.Append(Path.GetFileNameWithoutExtension(uri.LocalPath)); // Action name without ".ff"
            NameValueCollection parameters = HttpUtility.ParseQueryString(uri.Query);
            parameters.Remove("format");
            parameters.Remove("username");
            parameters.Remove("password");
            parameters.Remove("timestamp");
            parameters.Remove("channel");

            // Write out elements in canonical order
            foreach (var key in parameters.AllKeys.OrderBy(k => k))
            {
                sb.AppendFormat(
                    "_{0}={1}", 
                    key, 
                    parameters[key]
                );
            }

            sb.Append(".json");
            
            return sb.ToString();
        }

        /// <summary>
        /// Sets up the path of a directory where all saved responses can be found.
        /// </summary>
        /// <param name="responsePath">Path to directory with all responses</param>
        public static void SetupResponsePath(string responsePath)
        {
            PathToFiles = responsePath;
        }
    }
}
