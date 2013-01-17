using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omikron.FactFinder.Data
{
    public class AfterSearchNavigation : List<AsnGroup>
    {
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
