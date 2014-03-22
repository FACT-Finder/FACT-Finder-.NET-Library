using System;
using Omikron.FactFinder.Core;
using Omikron.FactFinder.Core.Server;
namespace Omikron.FactFinder.Adapter
{
    public class JsonScicAdapter : Omikron.FactFinder.Json.FF68.JsonScicAdapter
    {
        public JsonScicAdapter(DataProvider dataProvider, ParametersConverter parametersConverter, Omikron.FactFinder.Core.Client.UrlBuilder urlBuilder)
            : base(dataProvider, parametersConverter, urlBuilder)
        {
            throw new NotSupportedException("The SCIC-API is deprecated as of FACT-Finder 6.9");
        }
    }
}
