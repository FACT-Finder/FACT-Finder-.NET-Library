using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using log4net;
using Omikron.FactFinder.Configuration;

namespace Omikron.FactFinder
{
    public class UrlBuilder
    {
        private ParametersHandler ParametersHandler;
        private IUnixClock Clock;

        private IDictionary<string, string> Parameters;

        public string Action { get; set; }

        private static ILog log;

        static UrlBuilder()
        {
            log = LogManager.GetLogger(typeof(UrlBuilder));
        }

        public UrlBuilder(ParametersHandler parametersHandler, IUnixClock clock)
        {
            log.Debug("Initialize new UrlBuilder.");
            ParametersHandler = parametersHandler;
            Clock = clock;
            Parameters = new Dictionary<string, string>();
        }

        public void SetParameter(string name, string value)
        {
            Parameters[name] = value;
        }

        public IDictionary<string, string> GetParameters()
        {
            return Parameters;
        }

        public void SetParameters(IDictionary<string, string> parameters)
        {
            foreach (var parameter in parameters)
            {
                Parameters[parameter.Key] = parameter.Value;
            }
        }

        public void ResetParameters(IDictionary<string, string> parameters)
        {
            Parameters = new Dictionary<string, string>(parameters);
        }

        public void UnsetParameter(string name)
        {
            Parameters.Remove(name);
        }

        public Uri GetUrlWithoutAuthentication()
        {
            var config = ConnectionSection.GetSection();

            IDictionary<string, string> parameters = ParametersHandler.GetServerRequestParameters(Parameters);

            return new Uri(String.Format(
                "{0}://{1}:{2}/{3}/{4}?{5}",
                config.Protocol,
                config.ServerAddress,
                config.Port,
                config.Context,
                Action,
                parameters.ToUriQueryString()
            ));

        }

        public Uri GetUrlWithSimpleAuthentication()
        {
            var config = ConnectionSection.GetSection();

            IDictionary<string, string> parameters = ParametersHandler.GetServerRequestParameters(Parameters);

            parameters["timestamp"] = Clock.Now().ToString();
            parameters["username"] = config.Authentication.UserName;
            parameters["password"] = config.Authentication.Password.ToMD5();

            return new Uri(String.Format(
                "{0}://{1}:{2}/{3}/{4}?{5}",
                config.Protocol,
                config.ServerAddress,
                config.Port,
                config.Context,
                Action,
                parameters.ToUriQueryString()
            ));
        }

        public Uri GetUrlWithAdvancedAuthentication()
        {
            var config = ConnectionSection.GetSection();

            IDictionary<string, string> parameters = ParametersHandler.GetServerRequestParameters(Parameters);

            string timestamp = Clock.Now().ToString();

            parameters["timestamp"] = timestamp;
            parameters["username"] = config.Authentication.UserName;
            parameters["password"] = (
                config.Authentication.Prefix + 
                timestamp +
                config.Authentication.Password.ToMD5() +
                config.Authentication.Postfix
            ).ToMD5();

            return new Uri(String.Format(
                "{0}://{1}:{2}/{3}/{4}?{5}",
                config.Protocol,
                config.ServerAddress,
                config.Port,
                config.Context,
                Action,
                parameters.ToUriQueryString()
            ));
        }

        public Uri GetUrlWithHttpAuthentication()
        {
            var config = ConnectionSection.GetSection();

            IDictionary<string, string> parameters = ParametersHandler.GetServerRequestParameters(Parameters);

            return new Uri(String.Format(
                "{0}://{1}:{2}@{3}:{4}/{5}/{6}?{7}",
                config.Protocol,
                config.Authentication.UserName,
                config.Authentication.Password,
                config.ServerAddress,
                config.Port,
                config.Context,
                Action,
                parameters.ToUriQueryString()
            ));
        }
    }
}
