using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web.Script.Serialization;
using log4net;
using Omikron.FactFinder.Data;
using Omikron.FactFinder.Default;
using Omikron.FactFinder.Json.Helper;

namespace Omikron.FactFinder.Json.FF66
{
    public class JsonSearchAdapter : SearchAdapter
    {
        private static ILog log;

        static JsonSearchAdapter()
        {
            log = LogManager.GetLogger(typeof(JsonSearchAdapter));
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
                    _jsonData = jsonSerializer.Deserialize(base.Data, typeof(object));
                }
                return _jsonData;
            }
        }

        public JsonSearchAdapter(DataProvider dataProvider, ParametersHandler parametersHandler)
            : base(dataProvider, parametersHandler)
        {
            DataProvider.Type = RequestType.Search;
            DataProvider.SetParameter("format", "json");
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

        protected override bool CreateIsSearchTimedOut()
        {
            return (bool)JsonData.searchResult.timedOut;
        }

        protected override int CreateSearchTime()
        {
            return (int)JsonData.searchResult.searchTime;
        }

        protected override SearchStatus CreateSearchStatus()
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

        protected override SearchParameters CreateSearchParameters()
        {
            SearchParameters searchParameters;
            if (BreadCrumbTrail.Count > 0)
            {
                Uri url = BreadCrumbTrail.Last().Url;
                searchParameters = ParametersHandler.GetFactFinderParametersFromUrl(url);
            }
            else
            {
                searchParameters = ParametersHandler.GetFactFinderParameters();
            }
            return searchParameters;
        }

        protected override Data.ResultRecords CreateResult()
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

            return new ResultRecords(result, resultCount);
        }

        protected override IList<SuggestQuery> CreateSingleWordSearch()
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
                    ParametersHandler.GeneratePageLink(parameters),
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

        Record GetRecordFromRawData(dynamic recordData, int position)
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

        protected override AfterSearchNavigation CreateAsn()
        {
            var asn = new List<AsnGroup>();

            foreach (var groupData in JsonData.searchResult.groups)
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
                    Uri filterLink = ParametersHandler.GeneratePageLink(
                        ParametersHandler.ParseParametersFromString((string)element.searchParams)
                    );

                    AsnFilterItem filter;

                    if (asnGroup.Style == AsnGroupStyle.Slider)
                    {
                        NameValueCollection parameters = ParametersHandler.ParseParametersFromString((string)element.searchParams);

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

            foreach (var sortItemData in JsonData.searchResult.sortsList)
            {
                Uri sortLink = ParametersHandler.GeneratePageLink(
                    ParametersHandler.ParseParametersFromString(sortItemData.searchParams)
                );

                sorting.Add(new Item(
                    (string)sortItemData.description,
                    sortLink,
                    (bool)sortItemData.selected
                ));
            }

            return sorting;
        }

        protected override Paging CreatePaging()
        {
            var searchResultData = JsonData.searchResult;
            var paging = new Paging(
                (int)searchResultData.paging.currentPage,
                (int)searchResultData.paging.pageCount,
                BuildPageLink(searchResultData.paging.previousLink),
                BuildPageLink(searchResultData.paging.nextLink),
                ParametersHandler
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
                Uri pageLink = ParametersHandler.GeneratePageLink(
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

                options[value] = ParametersHandler.GeneratePageLink(
                    ParametersHandler.ParseParametersFromString((string)optionData.searchParams)
                );
            }

            return new ProductsPerPageOptions(options, defaultOption, selectedOption);
        }

        protected override IList<BreadCrumbItem> CreateBreadCrumbTrail()
        {
            var breadCrumbTrail = new List<BreadCrumbItem>();
            var breadCrumbs = JsonData.searchResult.breadCrumbTrailItems;
            int nBreadCrumbs = breadCrumbs.Count;
            if (nBreadCrumbs > 0)
            {
                int i = 1;
                foreach (var breadCrumbData in breadCrumbs)
                {
                    Uri link = ParametersHandler.GeneratePageLink(
                        ParametersHandler.ParseParametersFromString((string)breadCrumbData.searchParams)
                    );

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

        protected override CampaignList CreateCampaigns()
        {
            var campaigns = new List<Campaign>();

            if (JsonData.ContainsKey("campaigns"))
            {
                foreach (var campaignData in JsonData.campaigns)
                {
                    var campaign = CreateEmptyCampaignObject(campaignData);

                    FillCampaignObject(campaign, campaignData);

                    campaigns.Add(campaign);
                }
            }

            return new CampaignList(campaigns);
        }

        protected virtual Campaign CreateEmptyCampaignObject(dynamic campaignData)
        {
            return new Campaign(
                (string)campaignData.name,
                (string)campaignData.category,
                new Uri((string)campaignData.target.destination, UriKind.RelativeOrAbsolute)
            );
        }

        protected virtual void FillCampaignObject(Campaign campaign, dynamic campaignData)
        {
            FillCampaignWithFeedback(campaign, campaignData);
            FillCampaignWithPushedProducts(campaign, campaignData);
        }

        protected virtual void FillCampaignWithFeedback(Campaign campaign, dynamic campaignData)
        {
            if (campaignData.feedbackTexts.Count > 0)
            {
                var feedback = new Dictionary<string, string>();

                for (int i = 0; i < campaignData.feedbackTexts.Count; i++)
                {
                    feedback[i.ToString()] = (string)(campaignData.feedbackTexts[i]);
                }

                campaign.AddFeedback(feedback);
            }
        }
        protected virtual void FillCampaignWithPushedProducts(Campaign campaign, dynamic campaignData)
        {

            if (campaignData.pushedProducts.Count > 0)
            {
                var pushedProducts = new List<Record>();

                foreach (var recordData in campaignData.pushedProducts)
                {
                    var fieldName = (string)recordData.field;
                    var fieldValue = (string)recordData.name;
                    foreach (var pushedProductData in JsonData.pushedProducts)
                    {
                        if ((string)pushedProductData.record[fieldName] == fieldValue)
                        {
                            var record = new Record((string)pushedProductData.id);
                            record.SetFieldValues(pushedProductData.record.AsDictionary());
                            pushedProducts.Add(record);
                            break;
                        }
                    }
                }

                campaign.AddPushedProducts(pushedProducts);
            }
        }
    }
}
