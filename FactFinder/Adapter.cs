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
    }
}
