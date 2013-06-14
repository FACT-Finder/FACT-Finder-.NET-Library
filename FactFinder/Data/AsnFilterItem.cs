
using System;
namespace Omikron.FactFinder.Data
{
    public class AsnFilterItem : Item
    {
        public int MatchCount { get; private set; }
        public int ClusterLevel { get; private set; }
        public string PreviewImage { get; private set; }
        public string Field { get; private set; }

        public AsnFilterItem(
            string value,
            Uri url,
            bool selected = false,
            int matchCount = 0,
            int clusterLevel = 0,
            string previewImage = "",
            string field = ""
        )
            : base(value, url, selected)
        {
            MatchCount = matchCount;
            ClusterLevel = clusterLevel;
            PreviewImage = previewImage;
            Field = field;
        }

        public bool HasPreviewImage()
        {
            return PreviewImage.Length > 0;
        }
    }
}
