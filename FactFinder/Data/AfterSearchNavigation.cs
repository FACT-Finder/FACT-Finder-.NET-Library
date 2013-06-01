using System.Collections.Generic;

namespace Omikron.FactFinder.Data
{
    public class AfterSearchNavigation : List<AsnGroup>
    {
        public AfterSearchNavigation()
            : base()
        { }

        public AfterSearchNavigation(int capacity)
            : base(capacity)
        { }

        public AfterSearchNavigation(IEnumerable<AsnGroup> collection)
            : base(collection)
        { }

        public bool HasPreviewImages()
        {
            foreach (var group in this)
            {
                if (group.HasPreviewImages())
                    return true;
            }
            return false;
        }
    }
}
