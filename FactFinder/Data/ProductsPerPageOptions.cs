using System;
using System.Collections.Generic;
using System.Linq;

namespace Omikron.FactFinder.Data
{
    public class ProductsPerPageOptions : List<Item>
    {
        public Item DefaultOption { get; private set; }
        public Item SelectedOption { get; private set; }

        public ProductsPerPageOptions()
            : base()
        { }

        public ProductsPerPageOptions(
            IDictionary<int, Uri> options, 
            int defaultOption = -1, 
            int selectedOption = -1
        )
            : base(options.Count())
        {
            foreach (var option in options)
            {
                var item = new Item(option.Key.ToString(), option.Value, (option.Key == selectedOption));
                if (option.Key == selectedOption)
                    SelectedOption = item;
                if (option.Key == defaultOption)
                    DefaultOption = item;

                this.Add(item);
            }

            if (options.Count == 0) // FF 6.7 may occasionally return stripped down results
            {
                var defaultItem = new Item(defaultOption.ToString(), null, defaultOption == selectedOption);
                DefaultOption = defaultItem;
                this.Add(defaultItem);
                if (defaultOption == selectedOption)
                {
                    SelectedOption = defaultItem;
                }
                else
                {
                    var selectedItem = new Item(selectedOption.ToString(), null, true);
                    SelectedOption = selectedItem;
                    this.Add(selectedItem);
                }
            }

            if (DefaultOption == null && options.Count > 0)
                DefaultOption = this[0];
            if (SelectedOption == null && DefaultOption != null)
                SelectedOption = DefaultOption;
        }

        public bool IsDefaultOption(Item option)
        {
            return DefaultOption.Label == option.Label;
        }
    }
}
