using System;
using System.Collections.Generic;
using System.Collections.Specialized;
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

        private ParametersHandler ParametersHandler;

        public Paging(
            int currentPage, 
            int pageCount, 
            Item previousPageLink, 
            Item nextPageLink, 
            ParametersHandler parametersHandler,
            string sourceRefKey = null
        )
            : base()
        {
            CurrentPage = currentPage;
            PageCount = pageCount;
            ParametersHandler = parametersHandler;
            PreviousPageLink = previousPageLink;
            NextPageLink = nextPageLink;
            SourceRefKey = sourceRefKey;
        }

        public Uri GetPageLink(int pageNumber, string linkTarget = null)
        {
            if (pageNumber > PageCount || pageNumber < 1)
                return null;

            var parameters = new NameValueCollection();
            parameters["page"] = pageNumber.ToString();

            if (SourceRefKey != null)
                parameters["sourceRefKey"] = SourceRefKey;

            return ParametersHandler.GeneratePageLink(parameters, null, linkTarget);
        }
    }
}
