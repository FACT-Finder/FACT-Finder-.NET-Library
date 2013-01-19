using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omikron.FactFinder.Data
{
    public class SearchParameters
    {
        public string Query { get; private set; }
        public string Channel { get; private set; }
        public int ProductsPerPage { get; private set; }
        public int CurrentPage { get; private set; }
        public IDictionary<string, string> Filters { get; private set; }
        public IDictionary<string, string> Sortings { get; private set; }
        public bool IsNavigation { get; private set; }
        public int FollowSearch { get; private set; }

        public SearchParameters(
            string query,
            string channel,
            int productsPerPage = -1,
            int currentPage = 1,
            IDictionary<string, string> filters = null,
            IDictionary<string, string> sortings = null,
            bool isNavigation = false,
            int followSearch = 10000
        )
        {
            Query = query;
            Channel = channel;
            ProductsPerPage = productsPerPage;
            CurrentPage = currentPage;
            Filters = filters ?? new Dictionary<string, string>();
            Sortings = sortings ?? new Dictionary<string, string>();
            IsNavigation = isNavigation;
            FollowSearch = followSearch;
        }
    }
}
