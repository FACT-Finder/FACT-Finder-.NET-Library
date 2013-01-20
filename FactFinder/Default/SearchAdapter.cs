using System;
using System.Collections.Generic;
using System.Runtime.Serialization;
using Omikron.FactFinder.Data;

namespace Omikron.FactFinder.Default
{
    public class SearchAdapter : Adapter
    {
        private SearchStatus _articleNumberSearchStatus;
        public SearchStatus ArticleNumberSearchStatus
        {
            get
            {
                if (_articleNumberSearchStatus == SearchStatus.None)
                    _articleNumberSearchStatus = CreateArticleNumberSearchStatus();
                return _articleNumberSearchStatus;
            }
        }

        private bool _isArticleNumberSearch;
        public bool IsArticleNumberSearch
        {
            get
            {
                if (_isArticleNumberSearch == null)
                    _isArticleNumberSearch = CreateIsArticleNumberSearch();
                return _isArticleNumberSearch;
            }
        }

        private bool _isSearchTimedOut;
        public bool IsSearchTimedOut
        {
            get
            {
                if (_isSearchTimedOut == null)
                    _isSearchTimedOut = CreateIsSearchTimedOut();
                return _isSearchTimedOut;
            }
        }
        private SearchStatus _searchStatus;
        public SearchStatus SearchStatus
        {
            get
            {
                if (_searchStatus == SearchStatus.None)
                    _searchStatus = CreateSearchStatus();
                return _searchStatus;
            }
        }

        protected override string Data
        {
            get
            {
                var parameters = DataProvider.Parameters;
                if ((!parameters.ContainsKey("query") || parameters["query"].Length == 0) &&
                    (!parameters.ContainsKey("seoPath") || parameters["seoPath"].Length == 0) &&
                    (!parameters.ContainsKey("catalog") || parameters["catalog"] != "true"))
                {
                    throw new NoQueryException();
                }
                return DataProvider.Data;
            }
        }

        public SearchAdapter(DataProvider dataProvider)
            : base(dataProvider)
        { }
        
        protected virtual IList<TagQuery> CreateTagCloud()
        {
            return new List<TagQuery>();
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

        // Default behavior mimicks a successful search with empty results
        protected virtual SearchStatus CreateSearchStatus()
        {
            return SearchStatus.EmptyResult;
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
