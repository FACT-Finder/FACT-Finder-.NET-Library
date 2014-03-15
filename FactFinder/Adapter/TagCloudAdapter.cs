namespace Omikron.FactFinder.Adapter
{
    public class JsonTagCloudAdapter : Omikron.FactFinder.Json.FF68.JsonTagCloudAdapter
    {
        public JsonTagCloudAdapter(DataProvider dataProvider, ParametersHandler parametersHandler)
            : base(dataProvider, parametersHandler)
        {
            DataProvider.Type = RequestType.TagCloud;
        }
    }
}
