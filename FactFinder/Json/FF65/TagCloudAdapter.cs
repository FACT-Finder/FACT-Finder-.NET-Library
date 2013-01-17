using System.Collections.Generic;
using Omikron.FactFinder.Default;
using Omikron.FactFinder.Data;
using System.Web.Script.Serialization;

namespace Omikron.FactFinder.Json.FF65
{
    public class JsonTagCloudAdapter : TagCloudAdapter
    {
        public JsonTagCloudAdapter(DataProvider dataProvider)
            : base(dataProvider)
        {
            DataProvider.Type = RequestType.TagCloud;
            DataProvider.SetParameter("format", "json");
            DataProvider.SetParameter("do", "getTagCloud");
        }

        protected override IList<TagQuery> CreateTagCloud()
        {
            JavaScriptSerializer jsonSerializer = new JavaScriptSerializer();

            var jsonData = jsonSerializer.Deserialize<Dictionary<string, string>[]>(Data);

            var tagCloud = new List<TagQuery>();

            foreach (var tagQuery in jsonData)
            {
                tagCloud.Add(new TagQuery(
                    tagQuery["query"],
                    tagQuery["params"],
                    false,
                    float.Parse(tagQuery["weight"]),
                    int.Parse(tagQuery["searchCount"])
                ));
            }

            return tagCloud;
        }
    }
}
