using System;

namespace Omikron.FactFinder.Data
{
    public class TagQuery : Item
    {
        public float Weight { get; private set; }
        public int SearchCount { get; private set; }

        public TagQuery(string label, Uri url, bool selected = false, float weight = 0.0f, int searchCount = 0)
            : base(label, url, selected)
        {
            Weight = weight;
            SearchCount = searchCount;
        }
    }
}
