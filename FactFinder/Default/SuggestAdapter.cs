using System.Collections.Generic;
using log4net;
using Omikron.FactFinder.Data;

namespace Omikron.FactFinder.Default
{
    public class SuggestAdapter : Adapter
    {
        private IList<SuggestQuery> _suggestions;
        public IList<SuggestQuery> Suggestions
        {
            get
            {
                if (_suggestions == null)
                    _suggestions = CreateSuggestions();
                return _suggestions; 
            }
        }

        private static ILog log;

        static SuggestAdapter()
        {
            log = LogManager.GetLogger(typeof(SuggestAdapter));
        }

        public SuggestAdapter(DataProvider dataProvider, ParametersHandler parametersHandler)
            : base(dataProvider, parametersHandler)
        {
            log.Debug("Initialize new RecommendationAdapter.");
        }

        protected virtual IList<SuggestQuery> CreateSuggestions()
        {
            return new List<SuggestQuery>();
        }
    }
}
