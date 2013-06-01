using System;

namespace Omikron.FactFinder.Data
{
    public class SuggestQuery
    {
        public string Query { get; private set; }
        public Uri Url { get; private set; }
        public int HitCount { get; private set; }
        public string Type { get; private set; }
        public string ImageUrl { get; private set; }

        public SuggestQuery(
            string query,
            Uri url,
            int hitCount = 0,
            string type = "",
            string imageUrl = ""
        )
        {
            Query = query;
            Url = url;
            HitCount = hitCount;
            Type = type;
            ImageUrl = imageUrl;
        }
    }
}
