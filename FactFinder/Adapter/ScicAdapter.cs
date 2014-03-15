using System;
using Omikron.FactFinder.Core.Server;
using Omikron.FactFinder.Core;
namespace Omikron.FactFinder.Adapter
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
