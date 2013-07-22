using System.Collections.Generic;

namespace Omikron.FactFinder.Data
{
    public class ResultRecords : List<Record>
    {
        public int FoundRecordsCount { get; private set; }
        public string RefKey { get; private set; }

        public ResultRecords()
            : base()
        {
            FoundRecordsCount = 0;
            RefKey = null;
        }

        public ResultRecords(IEnumerable<Record> collection, int foundRecordsCount, string refKey = null)
            : base(collection)
        {
            FoundRecordsCount = foundRecordsCount;
            RefKey = refKey;
        }
    }
}
