using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omikron.FactFinder.Data
{
    public class SuggestQuery
    {
        public string Query { get; private set; }
        public string Url { get; private set; }
        public int HitCount { get; private set; }
        public string Type { get; private set; }
        public string ImageUrl { get; private set; }

        public SuggestQuery(
            string query,
            string url,
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
