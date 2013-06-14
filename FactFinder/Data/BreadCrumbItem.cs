using System;

namespace Omikron.FactFinder.Data
{
    public class BreadCrumbItem : Item
    {
        public BreadCrumbItemType Type { get; private set; }
        public string FieldName { get; private set; }
        public string FieldUnit { get; private set; }

        public BreadCrumbItem(
            string value,
            Uri url,
            bool selected = false,
            BreadCrumbItemType type = BreadCrumbItemType.Filter,
            string fieldName = "",
            string fieldUnit = ""
        )
            : base(value, url, selected)
        {
            Type = type;
            FieldName = fieldName;
            FieldUnit = fieldUnit;
        }
    }
}
