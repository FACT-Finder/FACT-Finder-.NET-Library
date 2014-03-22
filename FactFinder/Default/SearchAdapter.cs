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
        protected SearchStatus _articleNumberSearchStatus;
        protected bool? _isArticleNumberSearch;
        protected bool? _isSearchTimedOut;
        protected int? _searchTime;
        protected SearchStatus _searchStatus;
        protected ResultRecords _result;
        protected AfterSearchNavigation _asn;
        protected IList<Item> _sorting;
        protected Paging _paging;
        protected ProductsPerPageOptions _productsPerPageOptions;
        protected IList<BreadCrumbItem> _breadCrumbTrail;
        protected CampaignList _campaigns;
        protected IList<SuggestQuery> _singleWordSearch;
        protected SearchParameters _searchParameters;

        #region Property definitions with lazy getters
        public SearchStatus ArticleNumberSearchStatus
        {
            get
            {
                if (_articleNumberSearchStatus == SearchStatus.None)
                    _articleNumberSearchStatus = CreateArticleNumberSearchStatus();
                return _articleNumberSearchStatus;
            }
        }

        public bool IsArticleNumberSearch
        {
            get
            {
                if (_isArticleNumberSearch == null)
                    _isArticleNumberSearch = CreateIsArticleNumberSearch();
                return (bool)_isArticleNumberSearch;
            }
        }

        public bool IsSearchTimedOut
        {
            get
            {
                if (_isSearchTimedOut == null)
                    _isSearchTimedOut = CreateIsSearchTimedOut();
                return (bool)_isSearchTimedOut;
            }
        }

        public int SearchTime
        {
            get
            {
                if (_searchTime == null)
                    _searchTime = CreateSearchTime();
                return (int)_searchTime;
            }
        }

        public SearchStatus SearchStatus
        {
            get
            {
                if (_searchStatus == SearchStatus.None)
                    _searchStatus = CreateSearchStatus();
                return _searchStatus;
            }
        }

        public ResultRecords Result
        {
            get
            {
                if (_result == null)
                    _result = CreateResult();
                return _result;
            }
        }

        public AfterSearchNavigation Asn
        {
            get
            {
                if (_asn == null)
                    _asn = CreateAsn();
                return _asn;
            }
        }

        public IList<Item> Sorting
        {
            get
            {
                if (_sorting == null)
                    _sorting = CreateSorting();
                return _sorting;
            }
        }

        public Paging Paging
        {
            get
            {
                if (_paging == null)
                    _paging = CreatePaging();
                return _paging;
            }
        }

        public ProductsPerPageOptions ProductsPerPageOptions
        {
            get
            {
                if (_productsPerPageOptions == null)
                    _productsPerPageOptions = CreateProductsPerPageOptions();
                return _productsPerPageOptions;
            }
        }

        public IList<BreadCrumbItem> BreadCrumbTrail
        {
            get
            {
                if (_breadCrumbTrail == null)
                    _breadCrumbTrail = CreateBreadCrumbTrail();
                return _breadCrumbTrail;
            }
        }

        public CampaignList Campaigns
        {
            get
            {
                if (_campaigns == null)
                    _campaigns = CreateCampaigns();
                return _campaigns;
            }
        }

        public IList<SuggestQuery> SingleWordSearch
        {
            get
            {
                if (_singleWordSearch == null)
                    _singleWordSearch = CreateSingleWordSearch();
                return _singleWordSearch;
            }
        }

        public SearchParameters SearchParameters
        {
            get
            {
                if (_searchParameters == null)
                    _searchParameters = CreateSearchParameters();
                return _searchParameters;
            }
        }

        protected override string Data
        {
            get
            {
                var parameters = DataProvider.Parameters;
                if (String.IsNullOrEmpty(parameters["query"]) &&
                    String.IsNullOrEmpty(parameters["seoPath"]) &&
                    parameters["catalog"] != "true")
                {
                    throw new NoQueryException();
                }
                return DataProvider.Data;
            }
        }
        #endregion

        private static ILog log;

        static SearchAdapter()
        {
            log = LogManager.GetLogger(typeof(SearchAdapter));
        }

        public SearchAdapter(DataProvider dataProvider, ParametersConverter parametersConverter, Omikron.FactFinder.Core.Client.UrlBuilder urlBuilder)
            : base(dataProvider, parametersConverter, urlBuilder)
        {
            log.Debug("Initialize new SearchAdapter.");
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

        [Serializable]
        public class NoQueryException : Exception
        {
            public NoQueryException()
                : base()
            { }

            public NoQueryException(string message)
                : base(message)
            { }

            public NoQueryException(string message, Exception innerException)
                : base(message, innerException)
            { }

            public NoQueryException(SerializationInfo info, StreamingContext context)
                : base(info, context)
            { }
        }
    }
}
