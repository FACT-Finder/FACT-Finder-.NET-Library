using System.Collections.Generic;
using Omikron.FactFinder.Default;
using Omikron.FactFinder.Data;
using System.Web.Script.Serialization;
using Omikron.FactFinder.Json.Helper;
using System;
using log4net;
using System.Collections.Specialized;

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
