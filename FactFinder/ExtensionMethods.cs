using System.Collections.Generic;
using System;
using System.Collections.Specialized;
using System.Web;
using System.Text;
using System.Security.Cryptography;
using System.Linq;

namespace Omikron.FactFinder
{
    public static class ExtensionMethods
    {
        public static string ToUriQueryString(this IDictionary<string, string> dictionary)
        {
            NameValueCollection query = HttpUtility.ParseQueryString(string.Empty);
            
            foreach (KeyValuePair<string, string> pair in dictionary)
            {
                query[pair.Key] = pair.Value;
            }

            return query.ToString();
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
    }
}
