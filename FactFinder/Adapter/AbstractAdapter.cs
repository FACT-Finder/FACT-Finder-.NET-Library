using System.Collections.Specialized;
using log4net;
using Omikron.FactFinder.Core.Server;
using Omikron.FactFinder.Core;
namespace Omikron.FactFinder.Adapter
{
    public abstract class AbstractAdapter
    {
        protected DataProvider DataProvider;
        protected ParametersHandler ParametersHandler;

        private static ILog log;

        static AbstractAdapter()
        {
            log = LogManager.GetLogger(typeof(AbstractAdapter));
        }

        public AbstractAdapter(DataProvider dataProvider, ParametersHandler parametersHandler)
        {
            DataProvider = dataProvider;
            ParametersHandler = parametersHandler;
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
    }
}
