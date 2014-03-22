using System;
using System.Collections.Specialized;
using log4net;
using Omikron.FactFinder.Core.Client;
using Omikron.FactFinder.Core.Configuration;
using Omikron.FactFinder.Util;

namespace Omikron.FactFinder.Core.Server
{
    public class UrlBuilder
    {
        private ParametersConverter ParametersConverter;
        private IUnixClock Clock;

        private NameValueCollection Parameters;

        public string Action { get; set; }

        private static ILog log;

        static UrlBuilder()
        {
            log = LogManager.GetLogger(typeof(UrlBuilder));
        }

        public UrlBuilder(ParametersConverter parametersConverter, IUnixClock clock)
        {
            log.Debug("Initialize new UrlBuilder.");
            ParametersConverter = parametersConverter;
            Clock = clock;
            Parameters = new RequestParser().RequestParameters;
        }

        public void SetParameter(string name, string value)
        {
            Parameters[name] = value;
        }

        public NameValueCollection GetParameters()
        {
            return Parameters;
        }

        /// <summary>
        /// Sets the given parameters. All previous values for the given keys will be replaced.
        /// Unmentioned keys will remain.
        /// </summary>
        /// <param name="parameters">Key-value pairs to be added.</param>
        public void SetParameters(NameValueCollection parameters)
        {

            foreach (string key in parameters)
            {
                Parameters.Remove(key);
            }

            Parameters.Add(parameters);
        }

        public void ResetParameters(NameValueCollection parameters)
        {
            Parameters = new NameValueCollection(parameters);
        }

        public void UnsetParameter(string name)
        {
            Parameters.Remove(name);
        }

        public void UnsetAllParameters()
        {
            Parameters = new NameValueCollection();
        }

        public Uri GetUrlWithoutAuthentication()
        {
            var config = ConnectionSection.GetSection();

            NameValueCollection parameters = ParametersConverter.ClientToServerRequestParameters(Parameters);

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

            NameValueCollection parameters = ParametersConverter.ClientToServerRequestParameters(Parameters);

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

            NameValueCollection parameters = ParametersConverter.ClientToServerRequestParameters(Parameters);

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

            NameValueCollection parameters = ParametersConverter.ClientToServerRequestParameters(Parameters);

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
