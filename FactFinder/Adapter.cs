using System.Collections.Generic;
using log4net;
namespace Omikron.FactFinder
{
    public abstract class Adapter
    {
        protected DataProvider DataProvider;
        protected ParametersHandler ParametersHandler;

        private static ILog log;

        static Adapter()
        {
            log = LogManager.GetLogger(typeof(Adapter));
        }

        public Adapter(DataProvider dataProvider, ParametersHandler parametersHandler)
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

        public void SetParameters(IDictionary<string, string> parameters)
        {
            DataProvider.SetParameters(parameters);
        }
    }
}
