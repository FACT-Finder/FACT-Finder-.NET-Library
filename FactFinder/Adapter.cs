using System.Collections.Generic;
namespace Omikron.FactFinder
{
    public abstract class Adapter
    {
        protected DataProvider DataProvider;
        protected ParametersHandler ParametersHandler;

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
