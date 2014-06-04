using System;
using System.Collections.Specialized;
using log4net;
using Omikron.FactFinder.Core.Configuration;
using Omikron.FactFinder.Util;

namespace Omikron.FactFinder.Core.Server
{
    public class UrlBuilder
    {
        private IUnixClock Clock;

        private static ILog log;

        static UrlBuilder()
        {
            log = LogManager.GetLogger(typeof(UrlBuilder));
        }

        public UrlBuilder(IUnixClock clock)
        {
            log.Debug("Initialize new Server.UrlBuilder.");
            Clock = clock;
        }

        public Uri GetUrlWithoutAuthentication(RequestType action, NameValueCollection parameters)
        {
            EnsureChannelParameter(parameters);

            var config = ConnectionSection.GetSection();

            return new Uri(String.Format(
                "{0}://{1}:{2}/{3}/{4}?{5}",
                config.Protocol,
                config.ServerAddress,
                config.Port,
                config.Context,
                action,
                parameters.ToUriQueryString()
            ));

        }
        
        public Uri GetUrlWithAuthentication(RequestType action, NameValueCollection parameters)
        {
            var config = ConnectionSection.GetSection();

            switch (config.Authentication.Type)
            {
                case AuthenticationType.Http:
                    return GetUrlWithHttpAuthentication(action, parameters);
                case AuthenticationType.Basic:
                    return GetUrlWithSimpleAuthentication(action, parameters);
                case AuthenticationType.Advanced:
                    return GetUrlWithAdvancedAuthentication(action, parameters);
                default:
                    throw new Exception("Invalid authentication type configured.");
            }
        }

        public Uri GetUrlWithSimpleAuthentication(RequestType action, NameValueCollection parameters)
        {
            EnsureChannelParameter(parameters);

            var config = ConnectionSection.GetSection();

            parameters["timestamp"] = Clock.Now().ToString();
            parameters["username"] = config.Authentication.UserName;
            parameters["password"] = config.Authentication.Password.ToMD5();

            return new Uri(String.Format(
                "{0}://{1}:{2}/{3}/{4}?{5}",
                config.Protocol,
                config.ServerAddress,
                config.Port,
                config.Context,
                action,
                parameters.ToUriQueryString()
            ));
        }

        public Uri GetUrlWithAdvancedAuthentication(RequestType action, NameValueCollection parameters)
        {
            EnsureChannelParameter(parameters);

            var config = ConnectionSection.GetSection();

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
                action,
                parameters.ToUriQueryString()
            ));
        }

        public Uri GetUrlWithHttpAuthentication(RequestType action, NameValueCollection parameters)
        {
            EnsureChannelParameter(parameters);

            var config = ConnectionSection.GetSection();

            return new Uri(String.Format(
                "{0}://{1}:{2}@{3}:{4}/{5}/{6}?{7}",
                config.Protocol,
                config.Authentication.UserName,
                config.Authentication.Password,
                config.ServerAddress,
                config.Port,
                config.Context,
                action,
                parameters.ToUriQueryString()
            ));
        }

        public void EnsureChannelParameter(NameValueCollection parameters)
        {
            var config = ConnectionSection.GetSection();

            if (String.IsNullOrEmpty(parameters["channel"]) &&
                config.Channel != "")
            {
                parameters["channel"] = config.Channel;
            }
        }
    }
}
