using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omikron.FactFinder.Data
{
    public class SingleWordSearchQuery : SuggestQuery
    {
        public List<Record> PreviewRecords { get; private set; }

        public SingleWordSearchQuery(
            string query,
            string url,
            int hitCount = 0,
            string type = "",
            string imageUrl = ""
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
