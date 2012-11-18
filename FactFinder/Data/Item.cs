using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omikron.FactFinder.Data
{

    /// <summary>
    /// Represents any control item on the page. Associated with it are a label
    /// to display and a URL to follow, if it is clicked or selected.
    /// </summary>
    public class Item
    {
        public string Label { get; private set; }
        public string Url { get; private set; }
        public bool Selected { get; set; }

        public Item(string label, string url, bool selected = false)
        {
            Label = label;
            Url = url;
            Selected = selected;
        }
    }
}
