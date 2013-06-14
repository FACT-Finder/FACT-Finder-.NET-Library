using System;

namespace Omikron.FactFinder.Data
{
    public class AsnSliderItem : AsnFilterItem
    {
        public float AbsoluteMinimum { get; private set; }
        public float AbsoluteMaximum { get; private set; }
        public float SelectedMinimum { get; private set; }
        public float SelectedMaximum { get; private set; }

        public override Uri Url
        {
            get
            {
                return new Uri(String.Format("{0}&filter{1}={2}-{3}", base.Url, Field, SelectedMinimum, SelectedMaximum));
            }
        }

        // Use this to append the "left-right" part in JavaScript
        public string BaseUrl
        {
            get
            {
                return String.Format("{0}&filter{1}=", base.Url, Field);
            }
        }

        public override bool Selected
        {
            get
            {
                return SelectedMinimum != AbsoluteMinimum || SelectedMaximum != AbsoluteMaximum;
            }
        }

        public AsnSliderItem(
            Uri baseUrl,
            float absoluteMinimum = 0,
            float absoluteMaximum = 0,
            float selectedMinimum = 0,
            float selectedMaximum = 0,
            string field = ""
        )
            : base("", baseUrl, false, 0, 0, "", field)
        {
            AbsoluteMinimum = absoluteMinimum;
            AbsoluteMaximum = absoluteMaximum;
            SelectedMinimum = selectedMinimum;
            SelectedMaximum = selectedMaximum;
        }

        public void SetAbsoluteRange(float min, float max)
        {
            AbsoluteMinimum = min;
            AbsoluteMaximum = max;
        }

        public void SetSelectedRange(float min, float max)
        {
            SelectedMinimum = min;
            SelectedMaximum = max;
        }
    }
}
