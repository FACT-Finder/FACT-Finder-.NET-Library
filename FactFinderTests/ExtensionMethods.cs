using System.Collections.Generic;
using System;
using System.Collections.Specialized;
using System.Web;

namespace Omikron.FactFinderTests
{
    public static class ExtensionMethods
    {
        public static bool DictionaryEquals<TKey, TValue>(this IDictionary<TKey, TValue> first, IDictionary<TKey, TValue> second)
        {
            if (first == second) return true;
            if ((first == null) || (second == null)) return false;
            if (first.Count != second.Count) return false;

            var comparer = EqualityComparer<TValue>.Default;

            foreach (KeyValuePair<TKey, TValue> kvp in first)
            {
                TValue secondValue;
                if (!second.TryGetValue(kvp.Key, out secondValue)) return false;
                if (!comparer.Equals(kvp.Value, secondValue)) return false;
            }
            return true;
        }

        public static Dictionary<string, string> ToDictionary(this NameValueCollection collection)
        {
            var result = new Dictionary<string, string>();
            foreach(string key in collection.AllKeys)
            {
                result[key] = collection[key];
            }
            return result;
        }

        public static bool EqualsWithQueryString(this Uri first, Uri second)
        {
            if (Uri.Compare(
                    first,
                    second,
                    UriComponents.Scheme | UriComponents.UserInfo | UriComponents.Host | UriComponents.Port | UriComponents.Path,
                    UriFormat.Unescaped,
                    StringComparison.CurrentCultureIgnoreCase
                ) != 0)
                return false;

            Dictionary<string, string> firstQuery = HttpUtility.ParseQueryString(first.Query).ToDictionary();
            Dictionary<string, string> secondQuery = HttpUtility.ParseQueryString(second.Query).ToDictionary();

            return firstQuery.DictionaryEquals(secondQuery);
        }
    }
}
