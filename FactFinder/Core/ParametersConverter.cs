using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Web;
using log4net;
using Omikron.FactFinder.Core.Configuration;
using Omikron.FactFinder.Data;
using Omikron.FactFinder.Util;

namespace Omikron.FactFinder.Core
{
    public class ParametersConverter
    {
        private static ILog log;

        static ParametersConverter()
        {
            log = LogManager.GetLogger(typeof(ParametersConverter));
        }

        public ParametersConverter()
        {
            log.Debug("Initialize new ParametersConverter.");
        }

        public NameValueCollection ClientToServerRequestParameters(NameValueCollection clientParameters)
        {
            var config = ParametersSection.GetSection();

            var result = new NameValueCollection(clientParameters);

            RemoveIgnoredParameters(result, config.Server.IgnoreRules.ToList());
            ApplyParameterMappings(result, config.Server.MappingRules.ToDictionary());
            EnsureChannelParameter(result);
            AddRequiredParameters(result, config.Server.RequireRules.ToDictionary());

            return result;
        }

        public NameValueCollection ServerToClientRequestParameters(NameValueCollection serverParameters)
        {
            var config = ParametersSection.GetSection();

            var result = new NameValueCollection(serverParameters);

            RemoveIgnoredParameters(result, config.Client.IgnoreRules.ToList());
            ApplyParameterMappings(result, config.Client.MappingRules.ToDictionary());
            AddRequiredParameters(result, config.Client.RequireRules.ToDictionary());

            return result;
        }

        private void EnsureChannelParameter(NameValueCollection result)
        {
            if (string.IsNullOrEmpty(result["channel"]))
                result["channel"] = ConnectionSection.GetSection().Channel;
        }

        private void RemoveIgnoredParameters(NameValueCollection parameters, ICollection<string> ignoreList)
        {
            foreach (string parameterName in ignoreList)
            {
                parameters.Remove(parameterName);
            }

            // This is in a somewhat obscure location (mostly used to remove slider filters).
            // Maybe I should move this somewhere else?
            for (int i = parameters.AllKeys.Length - 1; i >= 0; i--)
            {
                if (parameters[i].Length == 0)
                    parameters.Remove(parameters.AllKeys[i]);
            }
        }

        private void ApplyParameterMappings(NameValueCollection parameters, IDictionary<string, string> mappings)
        {
            foreach (KeyValuePair<string, string> mapping in mappings)
            {
                string value = parameters[mapping.Key];
                if (value == null)
                    continue;
                parameters.Remove(mapping.Key);
                parameters[mapping.Value] = value;
            }
        }

        private void AddRequiredParameters(NameValueCollection parameters, IDictionary<string, string> requireList)
        {
            foreach (KeyValuePair<string, string> parameter in requireList)
            {
                if (parameters[parameter.Key] == null)
                    parameters.Add(parameter.Key, parameter.Value);
            }
        }
    }
}
