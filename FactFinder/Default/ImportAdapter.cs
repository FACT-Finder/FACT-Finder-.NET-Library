using log4net;
using Omikron.FactFinder.Adapter;
using Omikron.FactFinder.Core;
using Omikron.FactFinder.Core.Server;
using Omikron.FactFinder.Data;

namespace Omikron.FactFinder.Default
{
    public class ImportAdapter: AbstractAdapter
    {
        private static ILog log;

        static ImportAdapter()
        {
            log = LogManager.GetLogger(typeof(ImportAdapter));
        }

        public ImportAdapter(DataProvider dataProvider, ParametersConverter parametersConverter, Omikron.FactFinder.Core.Client.UrlBuilder urlBuilder)
            : base(dataProvider, parametersConverter, urlBuilder)
        {
            log.Debug("Initialize new ImportAdapter.");
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

        protected virtual object TriggerImport(ImportType importType, bool updateFiles)
        {
            return null;
        }
    }
}
