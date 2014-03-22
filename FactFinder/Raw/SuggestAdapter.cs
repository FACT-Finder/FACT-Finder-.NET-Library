using System;
using System.Collections.Generic;
using log4net;
using Omikron.FactFinder.Core;
using Omikron.FactFinder.Core.Server;
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

        public RawSuggestAdapter(DataProvider dataProvider, ParametersConverter parametersConverter, Omikron.FactFinder.Core.Client.UrlBuilder urlBuilder)
            : base(dataProvider, parametersConverter, urlBuilder)
        {
            DataProvider.Type = RequestType.Suggest;
        }

        protected override IList<SuggestQuery> CreateSuggestions()
        {
            throw new NotImplementedException();
        }
    }
}
