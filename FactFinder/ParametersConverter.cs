using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omikron.FactFinder
{
    public class ParametersConverter
    {
        private IConfiguration Configuration;

        public ParametersConverter(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IDictionary<string, string> GetServerRequestParams(IDictionary<string, string> pageParameters)
        {
            var result = new Dictionary<string, string>(pageParameters);

            RemoveIgnoredParameters(result);
            ApplyParameterMappings(result);
            AddRequiredParameters(result);

            return result;
        }

        private void RemoveIgnoredParameters(Dictionary<string, string> result)
        {
            foreach (string parameterName in Configuration.IgnoredServerParams)
            {
                result.Remove(parameterName);
            }
        }

        private void ApplyParameterMappings(Dictionary<string, string> result)
        {
            foreach (KeyValuePair<string, string> mapping in Configuration.ServerMappings)
            {
                string value;
                if (!result.TryGetValue(mapping.Key, out value))
                    continue;
                result.Remove(mapping.Key);
                result.Add(mapping.Value, value);
            }
        }

        private void AddRequiredParameters(Dictionary<string, string> result)
        {
            foreach (KeyValuePair<string, string> parameter in Configuration.RequiredServerParams)
            {
                string value;
                if (!result.TryGetValue(parameter.Key, out value))
                    result.Add(parameter.Key, parameter.Value);
            }
        }
    }
}
