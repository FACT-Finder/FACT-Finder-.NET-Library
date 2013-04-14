using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;

namespace Omikron.FactFinder.Data
{
    public class Paging : List<Item>
    {
        public int CurrentPage { get; private set; }
        public int PageCount { get; private set; }

        public Item PreviousPageLink { get; private set; }
        public Item NextPageLink { get; private set; }

        private ParametersHandler ParametersHandler;

        public Paging(int currentPage, int pageCount, Item previousPageLink, Item nextPageLink, ParametersHandler parametersHandler)
            : base()
        {
            CurrentPage = currentPage;
            PageCount = pageCount;
            ParametersHandler = parametersHandler;
            PreviousPageLink = previousPageLink;
            NextPageLink = nextPageLink;
        }

        public string GetPageLink(int pageNumber, string linkTarget = null)
        {
            if (pageNumber > PageCount || pageNumber < 1)
                return "";

            var parameters = new NameValueCollection();
            parameters["page"] = pageNumber.ToString();

            return ParametersHandler.GeneratePageLink(parameters, linkTarget);
        }
    }
}
