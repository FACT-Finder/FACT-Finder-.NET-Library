using log4net;
using Omikron.FactFinder.Core.Server;
using Omikron.FactFinder.Data;
namespace Omikron.FactFinder.Adapter
{
    public class Import : AbstractAdapter
    {
        protected dynamic JsonData { get { return ResponseContent; } }

        private static ILog log;

        static Import()
        {
            log = LogManager.GetLogger(typeof(Import));
        }

        public Import(Request request, Core.Client.UrlBuilder urlBuilder)
            : base(request, urlBuilder)
        {
            log.Debug("Initialize new Import adapter.");

            Parameters["format"] = "xml";

            // TODO: Implement XML processor
            UsePassthroughResponseContentProcessor();
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
            Parameters["download"] = updateFiles ? "true" : "false";

            switch (importType)
            {
            case ImportType.Suggest:
                Request.Action = RequestType.Import;
                Parameters["type"] = "suggest";
                break;
            case ImportType.Recommendations:
                Request.Action = RequestType.Recommendation;
                Parameters["do"] = "importData";
                break;
            case ImportType.Data:
            default:
                Request.Action = RequestType.Import;
                break;
            }

            var report = JsonData;

            switch (importType)
            {
            case ImportType.Suggest:
                Parameters.Remove("type");
                break;
            case ImportType.Recommendations:
                Parameters.Remove("do");
                break;
            }

            return report;
        }
    }
}
