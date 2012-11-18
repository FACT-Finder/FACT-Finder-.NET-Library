using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omikron.FactFinder.Data
{
    public class TagQuery : Item
    {
        public float Weight { get; private set; }
        public int SearchCount { get; private set; }

        public TagQuery(string label, string url, bool selected = false, float weight = 0.0f, int searchCount = 0)
            : base(label, url, selected)
        {
            Weight = weight;
            SearchCount = searchCount;
        }
    }
}
