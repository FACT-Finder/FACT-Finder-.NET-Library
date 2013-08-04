using System;
namespace Omikron.FactFinder.Json.FF69
{
    public class JsonScicAdapter : Omikron.FactFinder.Json.FF68.JsonScicAdapter
    {
        public JsonScicAdapter(DataProvider dataProvider, ParametersHandler parametersHandler)
            : base(dataProvider, parametersHandler)
        {
            throw new NotSupportedException("The SCIC-API is deprecated as of FACT-Finder 6.9");
        }
    }
}
