using System.Web.Script.Serialization;
using log4net;
using Omikron.FactFinder.Core;
using Omikron.FactFinder.Core.Server;
using Omikron.FactFinder.Data;
using Omikron.FactFinder.Util.Json;
namespace Omikron.FactFinder.Adapter
{
    public class Import : AbstractAdapter
    {
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

        private static ILog log;

        static Import()
        {
            log = LogManager.GetLogger(typeof(Import));
        }

        public Import(DataProvider dataProvider, ParametersConverter parametersConverter, Omikron.FactFinder.Core.Client.UrlBuilder urlBuilder)
            : base(dataProvider, parametersConverter, urlBuilder)
        {
            log.Debug("Initialize new ImportAdapter.");

            DataProvider.SetParameter("format", "json");
        }
        
        public object TriggerDataImport(bool updateFiles = false)
        {
            return TriggerImport(ImportType.Data, updateFiles);
        }

        public object TriggerSuggestImport(bool updateFiles = false)
        {
            return TriggerImport(ImportType.Suggest, updateFiles);
        }

        public object TriggerRecommendationImport(bool updateFiles = false)
        {
            return TriggerImport(ImportType.Recommendations, updateFiles);
        }

        protected object TriggerImport(ImportType importType, bool updateFiles)
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
