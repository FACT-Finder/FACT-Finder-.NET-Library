using System;
using System.Collections.Generic;
using System.Linq;

namespace Omikron.FactFinder.Data
{
    public class Record
    {
        public string ID { get; private set; }
        public float Similarity { get; private set; }
        public int Position { get; private set; }
        public int OriginalPosition { get; private set; }

        private IDictionary<string, object> _customFields;

        public string SeoPath { get; private set; }
        public IList<string> Keywords { get; private set; }

        public Record(
            string id, 
            float similarity = 100, 
            int position = 0, 
            int originalPosition = 0,
            IDictionary<string, object> fields = null,
            string seoPath = "",
            IList<string> keywords = null)
        {
            ID = id.Trim();
            // Clamp similarity to range 0 to 100
            Similarity = Math.Max(0, Math.Min(100,similarity));
            Position = position;
            OriginalPosition = originalPosition;
            SeoPath = seoPath;
            Keywords = keywords;

            if (fields != null)
            {
                _customFields = new Dictionary<string, object>(fields.Count);
                SetFieldValues(fields);
            }
            else
            {
                _customFields = new Dictionary<string, object>();
            }
        }

        public void AddKeyword(string keyword)
        {
            Keywords.Add(keyword);
        }

        public void AddKeywords(IList<string> keywords)
        {
            Keywords.Concat(keywords);
        }

        public object GetFieldValue(string fieldName)
        {
            return _customFields[fieldName];
        }

        public void SetFieldValue(string fieldName, object fieldValue)
        {
            _customFields[fieldName] = fieldValue;
        }

        public void SetFieldValues(IDictionary<string, object> fields)
        {
            foreach (var field in fields)
                SetFieldValue(field.Key, field.Value);
        }
    }
}
