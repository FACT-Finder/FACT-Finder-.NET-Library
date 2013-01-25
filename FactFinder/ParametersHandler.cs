using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Web;
using Omikron.FactFinder.Data;

namespace Omikron.FactFinder
{
    public class ParametersHandler
    {
        private IConfiguration Configuration;

        public ParametersHandler(IConfiguration configuration)
        {
            Configuration = configuration;
        }

        public IDictionary<string, string> GetServerRequestParameters(IDictionary<string, string> pageParameters)
        {
            var result = new Dictionary<string, string>(pageParameters);

            RemoveIgnoredParameters(result, Configuration.IgnoredServerParams);
            ApplyParameterMappings(result, Configuration.ServerMappings);
            AddChannelParameter(result);
            AddRequiredParameters(result, Configuration.RequiredServerParams);

            return result;
        }

        public IDictionary<string, string> GetPageRequestParameters(IDictionary<string, string> serverParameters)
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

        public string GeneratePageLink(IDictionary<string, string> parameters, string linkTarget = "")
        {
            if (linkTarget == "")
            {
                linkTarget = GetRequestTarget();
            }

            parameters = GetPageRequestParameters(parameters);

            return String.Format("{0}?{1}", linkTarget, parameters.ToUriQueryString());
        }

        private string GetRequestTarget()
        {
            return "dummyTarget";
        }

        public IDictionary<string, string>  ParseParametersFromString(string queryString)
        {
            NameValueCollection query = HttpUtility.ParseQueryString(queryString);
            return query.ToDictionary();
        }

        public SearchParameters GetFactFinderParametersFromString(string paramString)
        {
            throw new NotImplementedException();
        }

        public SearchParameters GetFactFinderParameters()
        {
            throw new NotImplementedException();
        }
    }
}
