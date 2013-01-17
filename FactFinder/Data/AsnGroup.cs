using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omikron.FactFinder.Data
{
    public class AsnGroup : List<AsnFilterItem>
    {
        public string Name { get; private set; }
        public int DetailedLinkCount { get; private set; }
        public string Unit { get; private set; }
        public AsnGroupStyle Style { get; private set; }

        public AsnGroup(
            IEnumerable<AsnFilterItem> filters,
            string name = "",
            int detailedLinkCount = 0,
            string unit = "",
            AsnGroupStyle style = AsnGroupStyle.Default
        )
            : base(filters)    
        {
            Name = name;
            DetailedLinkCount = detailedLinkCount;
            Unit = unit;
            Style = style;
        }

        public bool HasPreviewImages()
        {
            foreach (var item in this)
            {
                if (item.HasPreviewImage())
                    return true;
            }
            return false;
        }

        public bool HasSelectedItems()
        {
            foreach (var item in this)
            {
                if (item.Selected)
                    return true;
            }
            return false;
        }
    }
}
