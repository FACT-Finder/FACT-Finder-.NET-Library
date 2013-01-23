using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Web.Script.Serialization;
using Omikron.FactFinder.Data;
using Omikron.FactFinder.Default;
using Omikron.FactFinder.Json.Helper;

namespace Omikron.FactFinder.Json.FF65
{
    public class JsonSearchAdapter : SearchAdapter
    {
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
            return _isArticleNumberSearch;
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

            if (JsonData.records.Length() > 0)
            {
                resultCount = (int)JsonData.resultCount;
                var encodingHandler = new EncodingHandler();

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

                    encodingHandler.EncodeServerContentsForPage(fieldValues);

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

            if (JsonData.groups.Length > 0)
            {
                var encodingHandler = new EncodingHandler();

                foreach (var groupData in JsonData.groups)
                {
                    string groupName = encodingHandler.EncodeServerContentForPage((string)groupData.name);
                    string groupUnit = (string)groupData.unit;

                    var asnGroup = new AsnGroup(
                        new List<AsnFilterItem>(),
                        groupName,
                        (int)groupData.detailedLinks,
                        groupUnit,
                        GetAsnGroupStyleFromString((string)groupData.filterStyle)
                    );

                    // Merge element lists together
                    int nElements = groupData.elements.Length;
                    int nSelectedElements = groupData.selectedElements.Length;
                    dynamic[] elements = new dynamic[nElements + nSelectedElements];
                    Array.Copy(groupData.elements, 0, elements, 0, nElements);
                    Array.Copy(groupData.selectedElements, 0, elements, nElements, nSelectedElements);

                    foreach (var element in elements)
                    {
                        string filterLink = ParametersHandler.CreatePageLink(
                            ParametersHandler.ParseParametersFromResultString((string)element.searchParams)
                        );

                        AsnFilterItem filter;

                        if (asnGroup.Style == AsnGroupStyle.Slider)
                        {
                            IDictionary<string, string> parameters = ParametersHandler.ParseParametersFromResultString((string)element.searchParams)
                            filterLink += String.Format("&{0}=", parameters.Last());

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
                                encodingHandler.EncodeServerContentForPage((string)element.name),
                                filterLink,
                                (bool)element.selected,
                                (int)element.recordCount,
                                (int)element.clusterLevel,
                                (string)element.previewImageURL,
                                (string)element.associatedFieldName
                            );
                        }

                        asnGroup.Add(filter);
                    }

                    asn.Add(asnGroup);
                }
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

            if (JsonData.sortsList.Length > 0)
            {
                var encodingHandler = new EncodingHandler();

                foreach (var sortItemData in JsonData.sortsList)
                {
                    string sortLink = ParametersHandler.CreatePageLink(
                        ParametersHandler.ParseParametersFromResultString(sortItemData.searchParams)
                    );

                    sorting.Add(new Item(
                        encodingHandler.EncodeServerContentForPage(sortItemData.description),
                        sortLink,
                        (bool)sortItemData.selected
                    ));
                }
            }

            return sorting;
        }

        protected override Paging CreatePaging()
        {
            return new Paging(
                (int)JsonData.paging.currentPage,
                (int)JsonData.paging.pageCount,
                ParametersHandler
            );
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

                options[value] = ParametersHandler.CreatePageLink(
                    ParametersHandler.ParseParametersFromResultString((string)optionData.searchParams)
                );
            }

            return new ProductsPerPageOptions(options, defaultOption, selectedOption);
        }

        protected override IList<BreadCrumbItem> CreateBreadCrumbTrail()
        {
            var breadCrumbTrail = new List<BreadCrumbItem>();
            int nBreadCrumbs = JsonData.breadCrumbTrailItems.Length;
            if (nBreadCrumbs > 0)
            {
                var encodingHandler = new EncodingHandler();

                int i = 1;
                foreach (var breadCrumbData in JsonData.breadCrumbTrailItems)
                {
                    string link = ParametersHandler.CreatePageLink(
                        ParametersHandler.ParseParametersFromResultString((string)breadCrumbData.searchParams)
                    );

                    string fieldName = "";
                    string fieldUnit = "";

                    BreadCrumbItemType type = GetBreadCrumbItemTypeFromString((string)breadCrumbData.type);
                    if (type == BreadCrumbItemType.Filter)
                    {
                        fieldName = encodingHandler.EncodeServerContentForPage((string)breadCrumbData.associatedFieldName);
                        fieldUnit = ""; // TODO: Where is this data in JSON?
                    }
                    
                    breadCrumbTrail.Add(new BreadCrumbItem(
                        encodingHandler.EncodeServerContentForPage((string)breadCrumbData.value),
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
    }
}
