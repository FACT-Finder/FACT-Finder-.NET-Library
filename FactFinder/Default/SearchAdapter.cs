using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using log4net;
using Omikron.FactFinder.Adapter;
using Omikron.FactFinder.Core.Configuration;
using Omikron.FactFinder.Core;
using Omikron.FactFinder.Core.Server;
using Omikron.FactFinder.Data;
using System.Collections.Specialized;

namespace Omikron.FactFinder.Default
{
    public class SearchAdapter: AbstractAdapter
    {

        public SearchAdapter(DataProvider dataProvider, ParametersConverter parametersConverter, Omikron.FactFinder.Core.Client.UrlBuilder urlBuilder)
            : base(dataProvider, parametersConverter, urlBuilder)
        {
        }

        // Default behavior mimicks a non-article-number search
        protected virtual SearchStatus CreateArticleNumberSearchStatus()
        {
            return SearchStatus.NoResult;
        }

        // Default behavior mimicks a non-article-number search
        protected virtual bool CreateIsArticleNumberSearch()
        {
            return false;
        }

        // Default behavior mimicks a successful search with empty results
        protected virtual bool CreateIsSearchTimedOut()
        {
            return false;
        }

        // Default behavior mimicks a successful search (carried out in no time) with empty results
        protected virtual int CreateSearchTime()
        {
            return 0;
        }

        // Default behavior mimicks a successful search with empty results
        protected virtual SearchStatus CreateSearchStatus()
        {
            return SearchStatus.EmptyResult;
        }

        // Default behavior mimicks a successful search with empty results
        protected virtual ResultRecords CreateResult()
        {
            return new ResultRecords();
        }

        protected virtual AfterSearchNavigation CreateAsn()
        {
            return new AfterSearchNavigation();
        }

        protected virtual IList<Item> CreateSorting()
        {
            return new List<Item>();
        }

        protected virtual Paging CreatePaging()
        {
            return new Paging(1, 1, null, null, ParametersConverter);
        }

        protected virtual ProductsPerPageOptions CreateProductsPerPageOptions()
        {
            return new ProductsPerPageOptions();
        }

        protected virtual IList<BreadCrumbItem> CreateBreadCrumbTrail()
        {
            return new List<BreadCrumbItem>();
        }

        protected virtual CampaignList CreateCampaigns()
        {
            return new CampaignList();
        }

        protected virtual IList<SuggestQuery> CreateSingleWordSearch()
        {
            return new List<SuggestQuery>();
        }

        protected virtual SearchParameters CreateSearchParameters()
        {
            return new SearchParameters(new NameValueCollection());
        }
    }
}
