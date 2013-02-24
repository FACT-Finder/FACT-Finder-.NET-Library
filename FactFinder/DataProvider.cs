using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omikron.FactFinder
{
    public abstract class DataProvider
    {
        public virtual IDictionary<string, string> Parameters { get; protected set; }
        public virtual RequestType Type { get; set; }

        public virtual string Data { get; private set; }

        public DataProvider()
        {
            Parameters = new Dictionary<string, string>();
        }

        public virtual void SetParameters(IDictionary<string, string> parameters)
        {
            foreach (KeyValuePair<string, string> parameter in parameters)
            {
                Parameters[parameter.Key] = parameter.Value;
            }
        }

        public virtual void ResetParameters(IDictionary<string, string> parameters)
        {
            Parameters = parameters;
        }

        public virtual void SetParameter(KeyValuePair<string, string> parameter)
        {
            Parameters[parameter.Key] = parameter.Value;
        }

        public virtual void SetParameter(string name, string value)
        {
            Parameters[name] = value;
        }

        public virtual void UnsetParameter(string name)
        {
            Parameters.Remove(name);
        }
    }
}
