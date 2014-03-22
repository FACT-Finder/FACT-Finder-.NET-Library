using System.Web.Script.Serialization;
using log4net;
using Omikron.FactFinder.Core;
using Omikron.FactFinder.Core.Server;
using Omikron.FactFinder.Data;
using Omikron.FactFinder.Default;
using Omikron.FactFinder.Util.Json;

namespace Omikron.FactFinder.Json.FF66
{
    public class JsonImportAdapter : ImportAdapter
    {
        private static ILog log;

        static JsonImportAdapter()
        {
            log = LogManager.GetLogger(typeof(JsonImportAdapter));
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

        public JsonImportAdapter(DataProvider dataProvider, ParametersConverter parametersConverter, Omikron.FactFinder.Core.Client.UrlBuilder urlBuilder)
            : base(dataProvider, parametersConverter, urlBuilder)
        {
            DataProvider.SetParameter("format", "json");
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
