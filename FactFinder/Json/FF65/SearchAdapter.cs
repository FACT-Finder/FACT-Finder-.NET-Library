using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using log4net;
using Omikron.FactFinder.Data;
using Omikron.FactFinder.Default;
using Omikron.FactFinder.Json.Helper;

namespace Omikron.FactFinder.Json.FF65
{
    public class JsonSearchAdapter : SearchAdapter
    {
        private static IDictionary<string, string> sortingDescriptions;
        private static ILog log;

        static JsonSearchAdapter()
        {
            sortingDescriptions = new Dictionary<string, string>()
            {
                { "sort.relevanceDescription", "Relevance" },
                { "sort.titleAsc", "Name (A-Z)" },
                { "sort.titleDesc", "Name (Z-A)" },
                { "sort.priceAsc", "Increasing Price" },
                { "sort.priceDesc", "Decreasing Price" }
            };
            log = LogManager.GetLogger(typeof(JsonSearchAdapter));
        }

        public JsonSearchAdapter(DataProvider dataProvider, ParametersHandler parametersHandler)
            : base(dataProvider, parametersHandler)
        {
            DataProvider.Type = RequestType.Search;
            DataProvider.SetParameter("format", "json");
        }

        private dynamic _jsonData;
        protected dynamic JsonData
        {
            get
            {
                if (_jsonData == null)
                {
                    var jsonSerializer = new JavaScriptSerializer();
                    jsonSerializer.RegisterConverters(new[] { new DynamicJsonConverter() });
                    dynamic temp = jsonSerializer.Deserialize(base.Data, typeof(object));
                    _jsonData = temp.searchResult;
                }
                return _jsonData;
            }
        }

        protected override SearchStatus CreateArticleNumberSearchStatus()
        {
            if (_articleNumberSearchStatus == SearchStatus.None)
            {
                LoadArticleNumberSearchInformation();
            }
            return _articleNumberSearchStatus;
        }

        protected override bool CreateIsArticleNumberSearch()
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
            switch ((string)JsonData.resultArticleNumberStatus)
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

        protected override bool CreateIsSearchTimedOut()
        {
            return (bool)JsonData.timedOut;
        }

        protected override SearchStatus CreateSearchStatus()
        {
            switch ((string)JsonData.resultStatus)
            {
            case "nothingFound":
                return SearchStatus.EmptyResult;
            case "resultsFound":
                return SearchStatus.ResultsFound;
            default:
                return SearchStatus.NoResult;
            }
        }

        protected override SearchParameters CreateSearchParameters()
        {
            SearchParameters searchParameters;
            if (BreadCrumbTrail.Count > 0)
            {
                string paramString = BreadCrumbTrail.Last().Url;
                searchParameters = ParametersHandler.GetFactFinderParametersFromString(paramString);
            }
            else
            {
                searchParameters = ParametersHandler.GetFactFinderParameters();
            }
            return searchParameters;
        }

        protected override ResultRecords CreateResult()
        {
            var result = new List<Record>();
            int resultCount = 0;

            if (JsonData.records.Count > 0)
            {
                resultCount = (int)JsonData.resultCount;

                int positionOffset = (Paging.CurrentPage - 1) * Int32.Parse(ProductsPerPageOptions.SelectedOption.Label);

                int positionCounter = 1;

                foreach (var recordData in JsonData.records)
                {
                    int position = positionCounter + positionOffset;
                    int originalPosition = position;
                    ++positionCounter;

                    Dictionary<string, object> fieldValues = recordData.record.AsDictionary();

                    if (fieldValues.ContainsKey("__ORIG_POSITION__"))
                    {
                        originalPosition = Int32.Parse((string)fieldValues["__ORIG_POSITION__"]);
                        fieldValues.Remove("__ORIG_POSITION__");
                    }

                    result.Add(new Record(
                        recordData.id.ToString(),
                        (float)recordData.searchSimilarity,
                        position,
                        originalPosition,
                        fieldValues
                    ));
                }
            }

            return new ResultRecords(result, resultCount);
        }

        protected override AfterSearchNavigation CreateAsn()
        {
            var asn = new List<AsnGroup>();

            foreach (var groupData in JsonData.groups)
            {
                string groupName = (string)groupData.name;
                string groupUnit = (string)groupData.unit;

                var asnGroup = new AsnGroup(
                    new List<AsnFilterItem>(),
                    groupName,
                    (int)groupData.detailedLinks,
                    groupUnit,
                    GetAsnGroupStyleFromString((string)groupData.filterStyle)
                );

                var elements = groupData.selectedElements;
                elements.AddRange((IEnumerable<object>)groupData.elements);

                foreach (var element in elements)
                {
                    string filterLink = ParametersHandler.GeneratePageLink(
                        ParametersHandler.ParseParametersFromString((string)element.searchParams)
                    );

                    AsnFilterItem filter;

                    if (asnGroup.Style == AsnGroupStyle.Slider)
                    {
                        NameValueCollection parameters = ParametersHandler.ParseParametersFromString((string)element.searchParams);
                        filterLink += String.Format("&{0}=", parameters.AllKeys.Last());

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

                    asnGroup.Add(filter);
                }

                asn.Add(asnGroup);
            }

            return new AfterSearchNavigation(asn);
        }

        private AsnGroupStyle GetAsnGroupStyleFromString(string style)
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

        protected override IList<Item> CreateSorting()
        {
            var sorting = new List<Item>();

            foreach (var sortItemData in JsonData.sortsList)
            {
                string sortLink = ParametersHandler.GeneratePageLink(
                    ParametersHandler.ParseParametersFromString(sortItemData.searchParams)
                );

                sorting.Add(new Item(
                    sortingDescriptions[sortItemData.description],
                    sortLink,
                    (bool)sortItemData.selected
                ));
            }

            return sorting;
        }

        protected override Paging CreatePaging()
        {

            var paging = new Paging(
                (int)JsonData.paging.currentPage,
                (int)JsonData.paging.pageCount,
                BuildPageLink(JsonData.paging.previousLink),
                BuildPageLink(JsonData.paging.nextLink),
                ParametersHandler
            );

            foreach (var pageLinkData in JsonData.paging.pageLinks)
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
                string pageLink = ParametersHandler.GeneratePageLink(
                    ParametersHandler.ParseParametersFromString(linkData.searchParams)
                );

                link = new Item(
                    linkData.caption,
                    pageLink,
                    (bool)linkData.currentPage
                );
            }
            return link;
        }

        protected override ProductsPerPageOptions CreateProductsPerPageOptions()
        {
            var options = new Dictionary<int, string>();
            int defaultOption = -1;
            int selectedOption = -1;

            foreach(var optionData in JsonData.resultsPerPageList)
            {
                int value = (int)optionData.value;
                if ((bool)optionData.@default)
                    defaultOption = value;
                if ((bool)optionData.selected)
                    selectedOption = value;

                options[value] = ParametersHandler.GeneratePageLink(
                    ParametersHandler.ParseParametersFromString((string)optionData.searchParams)
                );
            }

            return new ProductsPerPageOptions(options, defaultOption, selectedOption);
        }

        protected override IList<BreadCrumbItem> CreateBreadCrumbTrail()
        {
            var breadCrumbTrail = new List<BreadCrumbItem>();
            int nBreadCrumbs = JsonData.breadCrumbTrailItems.Count;
            if (nBreadCrumbs > 0)
            {
                int i = 1;
                foreach (var breadCrumbData in JsonData.breadCrumbTrailItems)
                {
                    string link = ParametersHandler.GeneratePageLink(
                        ParametersHandler.ParseParametersFromString((string)breadCrumbData.searchParams)
                    );

                    string fieldName = "";
                    string fieldUnit = "";

                    BreadCrumbItemType type = GetBreadCrumbItemTypeFromString((string)breadCrumbData.type);
                    if (type == BreadCrumbItemType.Filter)
                    {
                        fieldName = (string)breadCrumbData.associatedFieldName;
                        fieldUnit = ""; // TODO: Where is this data in JSON?
                    }
                    
                    breadCrumbTrail.Add(new BreadCrumbItem(
                        (string)breadCrumbData.value,
                        link,
                        (i == nBreadCrumbs),
                        type,
                        fieldName,
                        fieldUnit
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

        protected override CampaignList CreateCampaigns()
        {
            var campaigns = new List<Campaign>();

            foreach (var campaignData in JsonData.campaigns)
            {
                var campaign = new Campaign(
                    (string)campaignData.name,
                    (string)campaignData.category,
                    (string)campaignData.target.destination
                );

                if (campaignData.feedbackTexts.Count > 0)
                {
                    var feedback = new Dictionary<string, string>();

                    foreach (var feedbackData in campaignData.feedbackTexts)
                    {
                        string nr = feedbackData.nr.ToString();
                        feedback[nr] = (string)feedbackData.text;
                    }

                    campaign.AddFeedback(feedback);
                }

                if (campaignData.pushedProductsRecords.Count > 0)
                {
                    var pushedProducts = new List<Record>();

                    foreach(var recordData in campaignData.pushedProductsRecords)
                    {
                        var record = new Record((string)recordData.id);
                        record.SetFieldValues(recordData.record.AsDictionary());
                        pushedProducts.Add(record);
                    }

                    campaign.AddPushedProducts(pushedProducts);
                }

                campaigns.Add(campaign);
            }

            return new CampaignList(campaigns);
        }

        protected override IList<SuggestQuery> CreateSingleWordSearch()
        {
            var singleWordSearch = new List<SuggestQuery>();

            if (JsonData.singleWordResults == null)
                return singleWordSearch;

            foreach (var swsData in JsonData.singleWordResults)
            {
                string query = (string)swsData.word;
                var parameters = new NameValueCollection()
                {
                    {"query", query}
                };
                singleWordSearch.Add(new SuggestQuery(
                    query,
                    ParametersHandler.GeneratePageLink(parameters),
                    (int)swsData.recordCount
                ));
            }

            return singleWordSearch;
        }
    }
}
