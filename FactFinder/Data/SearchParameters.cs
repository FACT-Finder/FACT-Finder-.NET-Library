using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using Omikron.FactFinder.Core.Configuration;

namespace Omikron.FactFinder.Data
{
    public class SearchParameters
    {
        public string Query { get; private set; }
        public string SeoPath { get; private set; }
        public string Channel { get; private set; }
        public int ProductsPerPage { get; private set; }
        public int CurrentPage { get; private set; }
        public IDictionary<string, string> Filters { get; private set; }
        public IDictionary<string, string> Sortings { get; private set; }
        public bool IsNavigation { get; private set; }
        public int FollowSearch { get; private set; }

        public SearchParameters(NameValueCollection parameters)
        {
            Query = parameters["query"] ?? "";
            SeoPath = parameters["seoPath"] ?? "";
            ProductsPerPage = Int32.Parse(parameters["productsPerPage"] ?? "-1");
            CurrentPage = Int32.Parse(parameters["page"] ?? "1");
            FollowSearch = Int32.Parse(parameters["followSearch"] ?? "0");
            IsNavigation = parameters["catalog"] == "true";

            Filters = new Dictionary<string, string>();
            Sortings = new Dictionary<string, string>();

            foreach (string key in parameters)
            {
                string value = parameters[key];
                if (key.StartsWith("filter"))
                {
                    Filters[key.Substring("filter".Length)] = value;
                }
                else if (key.StartsWith("sort") && (value == "asc" || value == "desc"))
                {
                    Sortings[key.Substring("sort".Length)] = value;
                }
            }

            var config = ConnectionSection.GetSection();
            Channel = config.Channel;
        }
    }
}
