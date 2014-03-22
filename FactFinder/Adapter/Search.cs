using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Runtime.Serialization;
using System.Web.Script.Serialization;
using log4net;
using Omikron.FactFinder.Core;
using Omikron.FactFinder.Core.Server;
using Omikron.FactFinder.Data;
using Omikron.FactFinder.Util;
using Omikron.FactFinder.Util.Json;

namespace Omikron.FactFinder.Adapter
{
    public class Search : AbstractAdapter
    {
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

        private dynamic _jsonData;
        protected dynamic JsonData
        {
            get
            {
                if (_jsonData == null)
                {
                    var jsonSerializer = new JavaScriptSerializer();
                    jsonSerializer.RegisterConverters(new[] { new DynamicJsonConverter() });
                    _jsonData = jsonSerializer.Deserialize(base.Data, typeof(object));
                }
                return _jsonData;
            }
        }

        private static ILog log;

        static Search()
        {
            log = LogManager.GetLogger(typeof(Search));
        }

        public Search(DataProvider dataProvider, ParametersConverter parametersConverter, Omikron.FactFinder.Core.Client.UrlBuilder urlBuilder)
            : base(dataProvider, parametersConverter, urlBuilder)
        {
            log.Debug("Initialize new SearchAdapter.");
            DataProvider.Type = RequestType.Search;
            DataProvider.SetParameter("format", "json");
        }

        protected SearchStatus CreateArticleNumberSearchStatus()
        {
            if (_articleNumberSearchStatus == SearchStatus.None)
            {
                LoadArticleNumberSearchInformation();
            }
            return _articleNumberSearchStatus;
        }

        protected bool CreateIsArticleNumberSearch()
        {
            if (_isArticleNumberSearch == null)
            {
                LoadArticleNumberSearchInformation();
            }
            return (bool)_isArticleNumberSearch;
        }

        private void LoadArticleNumberSearchInformation()
        {
            if (SearchStatus == SearchStatus.NoResult)
            {
                _isArticleNumberSearch = false;
                _articleNumberSearchStatus = SearchStatus.NoResult;
                return;
            }
            switch ((string)JsonData.searchResult.resultArticleNumberStatus)
            {
            case "nothingFound":
                _isArticleNumberSearch = true;
                _articleNumberSearchStatus = SearchStatus.EmptyResult;
                break;
            case "resultsFound":
                _isArticleNumberSearch = true;
                _articleNumberSearchStatus = SearchStatus.ResultsFound;
                break;
            case "noArticleNumberSearch":
            default:
                _isArticleNumberSearch = false;
                _articleNumberSearchStatus = SearchStatus.NoResult;
                break;
            }
        }

        protected bool CreateIsSearchTimedOut()
        {
            return (bool)JsonData.searchResult.timedOut;
        }

        protected int CreateSearchTime()
        {
            return (int)JsonData.searchResult.searchTime;
        }

        protected SearchStatus CreateSearchStatus()
        {
            switch ((string)JsonData.searchResult.resultStatus)
            {
            case "nothingFound":
                return SearchStatus.EmptyResult;
            case "resultsFound":
                return SearchStatus.ResultsFound;
            default:
                return SearchStatus.NoResult;
            }
        }

        protected SearchParameters CreateSearchParameters()
        {
            SearchParameters searchParameters;
            if (BreadCrumbTrail.Count > 0)
            {
                Uri url = BreadCrumbTrail.Last().Url;
                searchParameters = new SearchParameters(url.ToString().ToParameters());
            }
            else
            {
                searchParameters = new SearchParameters(new NameValueCollection());
            }
            return searchParameters;
        }

        protected CampaignList CreateCampaigns()
        {
            var campaigns = new List<Campaign>();

            if (JsonData.searchResult.ContainsKey("campaigns"))
            {
                foreach (var campaignData in JsonData.searchResult.campaigns)
                {
                    var campaign = CreateEmptyCampaignObject(campaignData);

                    FillCampaignObject(campaign, campaignData);

                    campaigns.Add(campaign);
                }
            }

            return new CampaignList(campaigns);
        }

        protected Campaign CreateEmptyCampaignObject(dynamic campaignData)
        {
            return new Campaign(
                (string)campaignData.name,
                "",
                new Uri((string)campaignData.target.destination, UriKind.RelativeOrAbsolute)
            );
        }

        protected void FillCampaignObject(Campaign campaign, dynamic campaignData)
        {
            switch ((string)campaignData.flavour)
            {
            case "FEEDBACK":
                FillCampaignWithFeedback(campaign, campaignData);
                FillCampaignWithPushedProducts(campaign, campaignData);
                break;

            case "ADVISOR":
                FillCampaignWithAdvisorData(campaign, campaignData);
                break;
            }
        }

        protected void FillCampaignWithFeedback(Campaign campaign, dynamic campaignData)
        {
            if (campaignData.feedbackTexts.Count > 0)
            {
                var feedback = new Dictionary<string, string>();

                foreach (var feedbackData in campaignData.feedbackTexts)
                {
                    string label = feedbackData.label.ToString();
                    if (label != "")
                        feedback[label] = (string)feedbackData.text;

                    string id = feedbackData.id.ToString();
                    if (id != "")
                        feedback[id] = (string)feedbackData.text;
                }

                campaign.AddFeedback(feedback);
            }
        }

        protected void FillCampaignWithPushedProducts(Campaign campaign, dynamic campaignData)
        {
            if (campaignData.pushedProductsRecords.Count > 0)
            {
                var pushedProducts = new List<Record>();

                foreach (var recordData in campaignData.pushedProductsRecords)
                {
                    var record = new Record((string)recordData.id);
                    record.SetFieldValues(recordData.record.AsDictionary());
                    pushedProducts.Add(record);
                }

                campaign.AddPushedProducts(pushedProducts);
            }
        }

        protected virtual void FillCampaignWithAdvisorData(Campaign campaign, dynamic campaignData)
        {
            var activeQuestions = new List<AdvisorQuestion>();

            // The active questions can still be empty if we have already moved down the whole question tree (while the search query still fulfills the campaign condition)
            foreach (var questionData in campaignData.activeQuestions)
            {
                activeQuestions.Add(LoadAdvisorQuestion(questionData));
            }

            campaign.AddActiveQuestions(activeQuestions);

            // Fetch advisor tree if it exists
            var advisorTree = new List<AdvisorQuestion>();

            foreach (var questionData in campaignData.activeQuestions)
            {
                activeQuestions.Add(LoadAdvisorQuestion(questionData, true));
            }

            campaign.AddToAdvisorTree(advisorTree);
        }

        protected AdvisorQuestion LoadAdvisorQuestion(dynamic questionData, bool recursive = false)
        {
            var answers = new List<AdvisorAnswer>();

            foreach (var answerData in questionData.answers)
            {
                string text = (string)answerData.text;
                Uri parameters = ConvertServerQueryToClientUrl((string)answerData.@params);

                var subquestions = new List<AdvisorQuestion>();
                if (recursive)
                    foreach (var subquestionData in answerData.questions)
                        subquestions.Add(LoadAdvisorQuestion(subquestionData, true));

                answers.Add(new AdvisorAnswer(
                    text,
                    parameters.Query,
                    subquestions
                ));
            }

            return new AdvisorQuestion((string)questionData.text, answers);
        }

        protected AfterSearchNavigation CreateAsn()
        {
            var asn = new List<AsnGroup>();

            foreach (var groupData in JsonData.searchResult.groups)
            {
                var asnGroup = CreateGroupInstance(groupData);

                var elements = groupData.selectedElements;
                elements.AddRange((IEnumerable<object>)groupData.elements);

                foreach (var element in elements)
                {
                    AsnFilterItem filter = CreateFilter(asnGroup, element);

                    asnGroup.Add(filter);
                }

                asn.Add(asnGroup);
            }

            return new AfterSearchNavigation(asn);
        }

        protected AsnGroup CreateGroupInstance(dynamic groupData)
        {
            string groupName = (string)groupData.name;
            string groupUnit = (string)groupData.unit;

            return new AsnGroup(
                new List<AsnFilterItem>(),
                groupName,
                (int)groupData.detailedLinks,
                groupUnit,
                GetAsnGroupStyleFromString((string)groupData.filterStyle)
            );
        }

        protected AsnFilterItem CreateFilter(dynamic asnGroup, dynamic element)
        {
            Uri filterLink = ConvertServerQueryToClientUrl(element.searchParams);

            AsnFilterItem filter;

            if (asnGroup.Style == AsnGroupStyle.Slider)
            {
                NameValueCollection parameters = ((string)element.searchParams).ToParameters();

                filter = new AsnSliderItem(
                    filterLink,
                    (float)element.absoluteMinValue,
                    (float)element.absoluteMaxValue,
                    (float)element.selectedMinValue,
                    (float)element.selectedMaxValue,
                    (string)element.associatedFieldName
                );
            }
            else
            {
                filter = new AsnFilterItem(
                    (string)element.name,
                    filterLink,
                    (bool)element.selected,
                    (int)element.recordCount,
                    (int)element.clusterLevel,
                    (string)(element.previewImageURL ?? ""),
                    (string)(element.associatedFieldName ?? "")
                );
            }
            return filter;
        }

        protected virtual AsnGroupStyle GetAsnGroupStyleFromString(string style)
        {
            switch (style)
            {
            case "TREE":
                return AsnGroupStyle.Tree;
            case "MULTISELECT":
                return AsnGroupStyle.MultiSelect;
            case "SLIDER":
                return AsnGroupStyle.Slider;
            case "COLOR":
                return AsnGroupStyle.Color;
            case "DEFAULT":
            default:
                return AsnGroupStyle.Default;
            }
        }

        protected IList<Item> CreateSorting()
        {
            var sorting = new List<Item>();

            foreach (var sortItemData in JsonData.searchResult.sortsList)
            {
                Uri sortLink = ConvertServerQueryToClientUrl(sortItemData.searchParams);

                sorting.Add(new Item(
                    (string)sortItemData.description,
                    sortLink,
                    (bool)sortItemData.selected
                ));
            }

            return sorting;
        }

        protected Paging CreatePaging()
        {
            var searchResultData = JsonData.searchResult;
            var paging = new Paging(
                (int)searchResultData.paging.currentPage,
                (int)searchResultData.paging.pageCount,
                BuildPageLink(searchResultData.paging.previousLink),
                BuildPageLink(searchResultData.paging.nextLink),
                ParametersConverter
            );

            foreach (var pageLinkData in searchResultData.paging.pageLinks)
            {
                paging.Add(BuildPageLink(pageLinkData));
            }

            return paging;
        }

        private Item BuildPageLink(dynamic linkData)
        {
            Item link = null;
            if (linkData != null)
            {
                Uri pageLink = ConvertServerQueryToClientUrl((string)linkData.searchParams);

                link = new Item(
                    linkData.caption,
                    pageLink,
                    (bool)linkData.currentPage
                );
            }
            return link;
        }

        protected ProductsPerPageOptions CreateProductsPerPageOptions()
        {
            var options = new Dictionary<int, Uri>();
            int defaultOption = -1;
            int selectedOption = -1;

            foreach (var optionData in JsonData.searchResult.resultsPerPageList)
            {
                int value = (int)optionData.value;
                if ((bool)optionData.@default)
                    defaultOption = value;
                if ((bool)optionData.selected)
                    selectedOption = value;

                options[value] = ConvertServerQueryToClientUrl((string)optionData.searchParams);
            }

            return new ProductsPerPageOptions(options, defaultOption, selectedOption);
        }

        protected IList<BreadCrumbItem> CreateBreadCrumbTrail()
        {
            var breadCrumbTrail = new List<BreadCrumbItem>();
            var breadCrumbs = JsonData.searchResult.breadCrumbTrailItems;
            int nBreadCrumbs = breadCrumbs.Count;
            if (nBreadCrumbs > 0)
            {
                int i = 1;
                foreach (var breadCrumbData in breadCrumbs)
                {
                    Uri link = ConvertServerQueryToClientUrl(breadCrumbData.searchParams);

                    string fieldName = "";

                    BreadCrumbItemType type = GetBreadCrumbItemTypeFromString((string)breadCrumbData.type);
                    if (type == BreadCrumbItemType.Filter)
                    {
                        fieldName = (string)breadCrumbData.associatedFieldName;
                    }

                    breadCrumbTrail.Add(new BreadCrumbItem(
                        (string)breadCrumbData.text,
                        link,
                        (i == nBreadCrumbs),
                        type,
                        fieldName,
                        "" // The JSON response does not have a separate field for the unit but instead includes
                        // it in the "text" field.
                    ));

                    ++i;
                }
            }

            return breadCrumbTrail;
        }

        private BreadCrumbItemType GetBreadCrumbItemTypeFromString(string type)
        {
            switch (type)
            {
            case "filter":
                return BreadCrumbItemType.Filter;
            case "search":
            default:
                return BreadCrumbItemType.Search;
            }
        }

        protected ResultRecords CreateResult()
        {
            var result = new List<Record>();
            int resultCount = 0;

            var searchResultData = JsonData.searchResult;

            if (searchResultData.records.Count > 0)
            {
                resultCount = (int)searchResultData.resultCount;

                int positionOffset = (Paging.CurrentPage - 1) * Int32.Parse(ProductsPerPageOptions.SelectedOption.Label);

                int positionCounter = 1;

                foreach (var recordData in searchResultData.records)
                {
                    int position = positionCounter + positionOffset;
                    ++positionCounter;

                    result.Add(GetRecordFromRawData(recordData, position));
                }
            }

            return new ResultRecords(result, resultCount, (string)searchResultData.refKey);
        }

        protected Record GetRecordFromRawData(dynamic recordData, int position)
        {

            int originalPosition = position;

            Dictionary<string, object> fieldValues = recordData.record.AsDictionary();

            if (fieldValues.ContainsKey("__ORIG_POSITION__"))
            {
                originalPosition = Int32.Parse((string)fieldValues["__ORIG_POSITION__"]);
                fieldValues.Remove("__ORIG_POSITION__");
            }

            var keywords = new List<string>();

            foreach (var keyword in recordData.keywords)
            {
                keywords.Add((string)keyword);
            }

            return new Record(
                recordData.id.ToString(),
                (float)recordData.searchSimilarity,
                position,
                originalPosition,
                fieldValues,
                (string)recordData.seoPath,
                keywords
            );
        }

        protected IList<SuggestQuery> CreateSingleWordSearch()
        {
            var singleWordSearch = new List<SuggestQuery>();

            if (JsonData.searchResult.singleWordResults == null)
                return singleWordSearch;

            foreach (var swsData in JsonData.searchResult.singleWordResults)
            {
                string query = (string)swsData.word;
                var parameters = new NameValueCollection()
                {
                    {"query", query}
                };

                var item = new SingleWordSearchQuery(
                    query,
                    UrlBuilder.GenerateUrl(parameters),
                    (int)swsData.recordCount
                );

                int position = 1;
                foreach (var recordData in swsData.previewRecords)
                {
                    item.AddPreviewRecord(GetRecordFromRawData(recordData, position));
                    ++position;
                }

                singleWordSearch.Add(item);
            }

            return singleWordSearch;
        }
    }
}
