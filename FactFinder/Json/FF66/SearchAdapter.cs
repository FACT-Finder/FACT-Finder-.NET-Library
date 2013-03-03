using System;
using System.Collections.Generic;
using log4net;
using Omikron.FactFinder.Data;
namespace Omikron.FactFinder.Json.FF66
{
    public class JsonSearchAdapter : Omikron.FactFinder.Json.FF65.JsonSearchAdapter
    {
        private static ILog log;

        static JsonSearchAdapter()
        {
            log = LogManager.GetLogger(typeof(JsonSearchAdapter));
        }

        public JsonSearchAdapter(DataProvider dataProvider, ParametersHandler parametersHandler)
            : base(dataProvider, parametersHandler)
        { }

        protected override Data.ResultRecords CreateResult()
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
                    ++positionCounter;

                    result.Add(GetRecordFromRawData(recordData, position));
                }
            }

            return new ResultRecords(result, resultCount);
        }

        protected override IList<SuggestQuery> CreateSingleWordSearch()
        {
            var singleWordSearch = new List<SuggestQuery>();

            foreach (var swsData in JsonData.singleWordResults)
            {
                string query = (string)swsData.word;
                var parameters = new Dictionary<string, string>()
                {
                    {"query", query}
                };

                var item = new SingleWordSearchQuery(
                    query,
                    ParametersHandler.GeneratePageLink(parameters),
                    (int)swsData.recordData
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
    }
}
