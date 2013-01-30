using System.Collections.Generic;
using Omikron.FactFinder.Default;
using Omikron.FactFinder.Data;
using System.Web.Script.Serialization;
using Omikron.FactFinder.Json.Helper;

namespace Omikron.FactFinder.Json.FF65
{
    public class JsonImportAdapter : ImportAdapter
    {
        public JsonImportAdapter(DataProvider dataProvider, ParametersHandler parametersHandler)
            : base(dataProvider, parametersHandler)
        {
            DataProvider.SetParameter("format", "json");
        }

        private dynamic _jsonData;
        protected dynamic JsonData
        {
            get
            {
                if (_jsonData == null)
                {
                    var jsonSerializer = new JavaScriptSerializer();
                    jsonSerializer.RegisterConverters(new[] { new DynamicJsonConverter() });
                    _jsonData = jsonSerializer.Deserialize(base.Data, typeof(object));
                }
                return _jsonData;
            }
        }

        protected override object TriggerImport(ImportType importType, bool updateFiles)
        {
            DataProvider.SetParameter("download", updateFiles ? "true" : "false");

            switch (importType)
            {
            case ImportType.Suggest:
                DataProvider.Type = RequestType.Import;
                DataProvider.SetParameter("type", "suggest");
                break;
            case ImportType.Recommendations:
                DataProvider.Type = RequestType.Recommendation;
                DataProvider.SetParameter("do", "importData");
                break;
            case ImportType.Data:
            default:
                DataProvider.Type = RequestType.Import;
                break;
            }

            var report = JsonData;

            switch (importType)
            {
            case ImportType.Suggest:
                DataProvider.UnsetParameter("type");
                break;
            case ImportType.Recommendations:
                DataProvider.UnsetParameter("do");
                break;
            }

            return report;
        }
    }
}
