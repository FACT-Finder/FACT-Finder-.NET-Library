using System;
using System.Collections.Generic;
using log4net;
using Omikron.FactFinder.Data;
using Omikron.FactFinder.Default;

namespace Omikron.FactFinder.Raw
{
    public class RawSuggestAdapter : SuggestAdapter
    {
        private static ILog log;

        static RawSuggestAdapter()
        {
            log = LogManager.GetLogger(typeof(RawSuggestAdapter));
        }

        public RawSuggestAdapter(DataProvider dataProvider, ParametersHandler parametersHandler)
            : base(dataProvider, parametersHandler)
        {
            DataProvider.Type = RequestType.Suggest;
        }

        protected override IList<SuggestQuery> CreateSuggestions()
        {
            throw new NotImplementedException();
        }
    }
}
