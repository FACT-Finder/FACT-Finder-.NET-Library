using System.Collections.Generic;
using log4net;
using Omikron.FactFinder.Adapter;
using Omikron.FactFinder.Core;
using Omikron.FactFinder.Core.Server;
using Omikron.FactFinder.Data;

namespace Omikron.FactFinder.Default
{
    public class SuggestAdapter: AbstractAdapter
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

        private string _rawSuggestions;
        public string RawSuggestions
        {
            get
            {
                if (_rawSuggestions == null)
                    _rawSuggestions = CreateRawSuggestions();
                return _rawSuggestions;
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
            log.Debug("Initialize new SuggestAdapter.");
        }

        protected virtual IList<SuggestQuery> CreateSuggestions()
        {
            return new List<SuggestQuery>();
        }

        protected virtual string CreateRawSuggestions()
        {
            return Data;
        }
    }
}
