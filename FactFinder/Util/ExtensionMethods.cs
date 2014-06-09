using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Web;

namespace Omikron.FactFinder.Util
{
    public static class ExtensionMethods
    {
        public static string ToUriQueryString(this IDictionary<string, string> dictionary)
        {
            StringBuilder queryString = new StringBuilder();
            foreach (KeyValuePair<string, string> pair in dictionary)
            {
                if (!String.IsNullOrEmpty(pair.Key) && !String.IsNullOrEmpty(pair.Value))
                {
                    if (queryString.Length > 0)
                    { queryString.Append("&"); }
                    queryString.Append(HttpUtility.UrlEncode(pair.Key)).Append("=").Append(HttpUtility.UrlEncode(pair.Value));
                }
            }

            return queryString.ToString();
        }

        public static string ToUriQueryString(this NameValueCollection nvc)
        {
            StringBuilder queryString = new StringBuilder();
            // Copy all keys over and make sure to handle multi-value keys properly
            foreach (string key in nvc)
                if (!String.IsNullOrEmpty(key))
                {
                    string encodedKey = HttpUtility.UrlEncode(key);
                    foreach (string value in nvc.GetValues(key))
                        if (!String.IsNullOrEmpty(value))
                        {
                            if (queryString.Length > 0)
                            { queryString.Append("&"); }
                            queryString.Append(encodedKey).Append("=").Append(HttpUtility.UrlEncode(value));
                        }
                }
            return queryString.ToString();
        }

        public static IDictionary<string, string> ToDictionary(this NameValueCollection source)
        {
            return source.Cast<string>()
                         .Select(s => new { Key = s, Value = source[s] })
                         .ToDictionary(p => p.Key, p => p.Value);
        }

        public static string ToMD5(this string input)
        {
            if ((input == null) || (input.Length == 0))
            {
                return string.Empty;
            }

            byte[] password = Encoding.Default.GetBytes(input);
            byte[] result = new MD5CryptoServiceProvider().ComputeHash(password);

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < result.Length; i++)
            {
                sb.Append(result[i].ToString("x2"));
            }
            return sb.ToString();
        }

        // We can't add a static extension method NameValueCollection.FromParameters(), so
        // we'll add a conversion method to String instead.
        public static NameValueCollection ToParameters(this string queryString)
        {
            char[] querySeparator = { '?' };
            return HttpUtility.ParseQueryString(queryString.Split(querySeparator).Last());
        }
    }
}
