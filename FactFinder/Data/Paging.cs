using System;
using System.Collections.Generic;
using Omikron.FactFinder.Core;

namespace Omikron.FactFinder.Data
{
    public class Paging : List<Item>
    {
        public int CurrentPage { get; private set; }
        public int PageCount { get; private set; }

        public Item PreviousPageLink { get; private set; }
        public Item NextPageLink { get; private set; }

        private string SourceRefKey;

        private ParametersConverter ParametersConverter;

        public Paging(
            int currentPage, 
            int pageCount, 
            Item previousPageLink, 
            Item nextPageLink, 
            ParametersConverter parametersConverter,
            string sourceRefKey = null
        )
            : base()
        {
            CurrentPage = currentPage;
            PageCount = pageCount;
            ParametersConverter = parametersConverter;
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
