using System;
using System.Collections.Generic;

namespace Omikron.FactFinder.Data
{
    public class SingleWordSearchQuery : SuggestQuery
    {
        public List<Record> PreviewRecords { get; private set; }

        public SingleWordSearchQuery(
            string query,
            Uri url,
            int hitCount = 0,
            string type = "",
            Uri imageUrl = null
        )
            : base(query, url, hitCount, type, imageUrl)
        {
            PreviewRecords = new List<Record>();
        }

        public void AddPreviewRecords(IEnumerable<Record> previewRecords)
        {
            PreviewRecords.AddRange(previewRecords);
        }

        public void AddPreviewRecord(Record record)
        {
            PreviewRecords.Add(record);
        }
    }
}
