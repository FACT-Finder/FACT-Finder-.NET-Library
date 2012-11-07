using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omikron.FactFinder
{
    public abstract class DataProvider
    {
        public IConfiguration Configuration { get; protected set; }
        public IDictionary<string, string> Parameters { get; protected set; }
        public RequestType Type { get; set; }

        public DataProvider(IConfiguration configuration)
        {
            Parameters = new Dictionary<string, string>();
            Configuration = configuration;
        }

        abstract public string GetData();

        public void SetParameters(IDictionary<string, string> parameters)
        {
            foreach (KeyValuePair<string, string> parameter in parameters)
            {
                Parameters[parameter.Key] = parameter.Value;
            }
        }

        public void ResetParameters(IDictionary<string, string> parameters)
        {
            Parameters = parameters;
        }

        public void SetParameter(KeyValuePair<string, string> parameter)
        {
            Parameters[parameter.Key] = parameter.Value;
        }

        public void SetParameter(string name, string value)
        {
            Parameters[name] = value;
        }

        public void UnsetParameter(string name)
        {
            Parameters.Remove(name);
        }

        public void SetConfiguration(IConfiguration configuration)
        {
            Configuration = configuration;
        }
    }
}
