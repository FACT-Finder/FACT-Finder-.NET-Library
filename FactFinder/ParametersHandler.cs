using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Web;
using log4net;
using Omikron.FactFinder.Configuration;
using Omikron.FactFinder.Data;

namespace Omikron.FactFinder
{
    public class ParametersHandler
    {
        private static ILog log;

        private NameValueCollection _requestParameters;
        protected NameValueCollection RequestParameters
        {
            get
            {
                if (_requestParameters == null)
                {
                    _requestParameters = HttpContext.Current.Request.QueryString;
                    _requestParameters.Add(HttpContext.Current.Request.Form);
                }
                return _requestParameters;
            }
        }

        private string _requestTarget;
        public string RequestTarget
        {
            get
            {
                if (_requestTarget == null)
                {
                    _requestTarget = HttpContext.Current.Request.Url.LocalPath;
                }
                return _requestTarget;
            }
        }

        static ParametersHandler()
        {
            log = LogManager.GetLogger(typeof(ParametersHandler));
        }

        public ParametersHandler()
        {
            log.Debug("Initialize new ParametersHandler.");
        }

        public string GetRequestParam(string key, string defaultValue = null)
        {
            return RequestParameters[key] ?? defaultValue;
        }

        public NameValueCollection ClientToServerRequestParameters(NameValueCollection clientParameters)
        {
            var config = ParametersSection.GetSection();

            var result = new NameValueCollection(clientParameters);

            RemoveIgnoredParameters(result, config.Server.IgnoreRules.ToList());
            ApplyParameterMappings(result, config.Server.MappingRules.ToDictionary());
            AddChannelParameter(result);
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

        private void AddChannelParameter(NameValueCollection result)
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

        public string GeneratePageLink(NameValueCollection parameters, string linkTarget = "")
        {
            if (linkTarget == "")
            {
                linkTarget = RequestTarget;
            }

            parameters = ServerToClientRequestParameters(parameters);

            return String.Format("{0}?{1}", linkTarget, parameters.ToUriQueryString());
        }

        public NameValueCollection ParseParametersFromString(string queryString)
        {
            char[] querySeparator = { '?' };
            return HttpUtility.ParseQueryString(queryString.Split(querySeparator).Last());
        }

        public SearchParameters GetFactFinderParametersFromString(string paramString)
        {
            throw new NotImplementedException();
        }

        public SearchParameters GetFactFinderParameters(NameValueCollection parameters = null)
        {
            if (parameters == null)
            {
                parameters = GetRequestParamsForServer();
            }

            var filters = new Dictionary<string, string>();
            var sortings = new Dictionary<string, string>();

            foreach (string key in parameters)
            {
                string value = parameters[key];
                if (key.StartsWith("filter"))
                {
                    filters[key.Substring("filter".Length)] = value;
                }
                else if (key.StartsWith("sort") && (value == "asc" || value == "desc"))
                {
                    sortings[key.Substring("sort".Length)] = value;
                }
            }

            var config = ConnectionSection.GetSection();

            return new SearchParameters(
                parameters["query"] ?? "",
                config.Channel,
                Int32.Parse(parameters["productsPerPage"] ?? "-1"),
                Int32.Parse(parameters["page"] ?? "1"),
                filters,
                sortings,
                parameters["catalog"] == "true",
                Int32.Parse(parameters["followSearch"] ?? "10000")
            );
        }

        public NameValueCollection GetRequestParamsForServer(NameValueCollection parameters = null)
        {
            if (parameters == null)
            {
                parameters = RequestParameters;
            }

            return ClientToServerRequestParameters(parameters);
        }
    }
}
