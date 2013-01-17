using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omikron.FactFinder.Data
{
    public class Result : List<Record>
    {
        public int FoundRecordsCount { get; private set; }

        public Result()
            : base()
        {
            FoundRecordsCount = 0;
        }

        public Result(IEnumerable<Record> collection, int foundRecordsCount)
            : base(collection)
        {
            FoundRecordsCount = foundRecordsCount;
        }
    }
}
