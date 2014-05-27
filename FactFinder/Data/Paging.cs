using System;
using System.Collections.Generic;

namespace Omikron.FactFinder.Data
{
    public class Paging : List<Item>
    {
        public int CurrentPage { get; private set; }
        public int PageCount { get; private set; }

        public Item PreviousPageLink { get; private set; }
        public Item NextPageLink { get; private set; }

        private string SourceRefKey;

        public Paging(
            int currentPage, 
            int pageCount, 
            Item previousPageLink, 
            Item nextPageLink,
            string sourceRefKey = null
        )
            : base()
        {
            CurrentPage = currentPage;
            PageCount = pageCount;
            PreviousPageLink = previousPageLink;
            NextPageLink = nextPageLink;
            SourceRefKey = sourceRefKey;
        }

        public Uri GetPageLink(int pageNumber, string linkTarget = null)
        {
            throw new NotImplementedException();
        }
    }
}
