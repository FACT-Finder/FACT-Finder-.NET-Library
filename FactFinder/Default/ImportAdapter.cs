using System.Collections.Generic;
using Omikron.FactFinder.Data;

namespace Omikron.FactFinder.Default
{
    public class ImportAdapter : Adapter
    {
        public ImportAdapter(DataProvider dataProvider, ParametersHandler parametersHandler)
            : base(dataProvider, parametersHandler)
        { }
        
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
