using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omikron.FactFinder.Data
{
    public class ResultRecords : List<Record>
    {
        public int FoundRecordsCount { get; private set; }

        public ResultRecords()
            : base()
        {
            FoundRecordsCount = 0;
        }

        public ResultRecords(IEnumerable<Record> collection, int foundRecordsCount)
            : base(collection)
        {
            FoundRecordsCount = foundRecordsCount;
        }
    }
}
