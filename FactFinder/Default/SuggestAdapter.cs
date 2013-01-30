using System.Collections.Generic;
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

        public SuggestAdapter(DataProvider dataProvider, ParametersHandler parametersHandler)
            : base(dataProvider, parametersHandler)
        { }

        protected virtual IList<SuggestQuery> CreateSuggestions()
        {
            return new List<SuggestQuery>();
        }
    }
}
