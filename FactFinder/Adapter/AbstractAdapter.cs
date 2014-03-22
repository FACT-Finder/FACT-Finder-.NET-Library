using System;
using System.Collections.Specialized;
using log4net;
using Omikron.FactFinder.Core;
using Omikron.FactFinder.Core.Client;
using Omikron.FactFinder.Core.Server;
using Omikron.FactFinder.Util;

namespace Omikron.FactFinder.Adapter
{
    public abstract class AbstractAdapter
    {
        protected DataProvider DataProvider;
        protected ParametersConverter ParametersConverter;
        protected Omikron.FactFinder.Core.Client.UrlBuilder UrlBuilder;

        private static ILog log;

        static AbstractAdapter()
        {
            log = LogManager.GetLogger(typeof(AbstractAdapter));
        }

        public AbstractAdapter(DataProvider dataProvider, ParametersConverter parametersConverter, Omikron.FactFinder.Core.Client.UrlBuilder urlBuilder)
        {
            DataProvider = dataProvider;
            ParametersConverter = parametersConverter;
            UrlBuilder = urlBuilder;
        }

        protected virtual string Data
        {
            get
            {
                return DataProvider.Data;
            }
        }

        public void SetParameter(string name, string value)
        {
            DataProvider.SetParameter(name, value);
        }

        public void SetParameters(NameValueCollection parameters)
        {
            DataProvider.SetParameters(parameters);
        }

        protected Uri ConvertServerQueryToClientUrl(string query)
        {
            return UrlBuilder.GenerateUrl(query.ToParameters());
        }
    }
}
