using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omikron.FactFinder
{
    public class ParametersHandler
    {
        private IConfiguration Configuration;

        public ParametersHandler(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IDictionary<string, string> GetServerRequestParams(IDictionary<string, string> pageParameters)
        {
            var result = new Dictionary<string, string>(pageParameters);

            RemoveIgnoredParameters(result, Configuration.IgnoredServerParams);
            ApplyParameterMappings(result, Configuration.ServerMappings);
            AddChannelParameter(result);
            AddRequiredParameters(result, Configuration.RequiredServerParams);

            return result;
        }

        public IDictionary<string, string> GetPageRequestParams(IDictionary<string, string> serverParameters)
        {
            var result = new Dictionary<string, string>(serverParameters);

            RemoveIgnoredParameters(result, Configuration.IgnoredPageParams);
            ApplyParameterMappings(result, Configuration.PageMappings);
            AddRequiredParameters(result, Configuration.RequiredPageParams);

            return result;
        }

        private void AddChannelParameter(Dictionary<string, string> result)
        {
            if (!result.ContainsKey("channel") || result["channel"].Length == 0)
                result["channel"] = Configuration.Channel;
        }

        private void RemoveIgnoredParameters(IDictionary<string, string> parameters, ICollection<string> ignoreList)
        {
            foreach (string parameterName in ignoreList)
            {
                parameters.Remove(parameterName);
            }
        }

        private void ApplyParameterMappings(IDictionary<string, string> parameters, IDictionary<string, string> mappings)
        {
            foreach (KeyValuePair<string, string> mapping in mappings)
            {
                string value;
                if (!parameters.TryGetValue(mapping.Key, out value))
                    continue;
                parameters.Remove(mapping.Key);
                parameters[mapping.Value] = value;
            }
        }

        private void AddRequiredParameters(IDictionary<string, string> parameters, IDictionary<string, string> requireList)
        {
            foreach (KeyValuePair<string, string> parameter in requireList)
            {
                if (!parameters.ContainsKey(parameter.Key))
                    parameters.Add(parameter);
            }
        }

        public string GeneratePageLink(Dictionary<string, string> parameters, string linkTarget)
        {
            throw new NotImplementedException();
        }
    }
}
