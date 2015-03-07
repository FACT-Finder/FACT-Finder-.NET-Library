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

        public Paging(
            int currentPage, 
            int pageCount, 
            Item previousPageLink, 
            Item nextPageLink
        )
            : base()
        {
            CurrentPage = currentPage;
            PageCount = pageCount;
            PreviousPageLink = previousPageLink;
            NextPageLink = nextPageLink;
        }

        public Uri GetPageLink(int pageNumber, string linkTarget = null)
        {
            throw new NotImplementedException();
        }
    }
}
