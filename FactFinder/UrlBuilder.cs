using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omikron.FactFinder
{
    public class UrlBuilder
    {
        private IConfiguration Configuration;
        private ParametersHandler ParametersHandler;
        private IUnixClock Clock;

        private IDictionary<string, string> Parameters;

        public string Action { get; set; }

        public UrlBuilder(IConfiguration configuration, ParametersHandler parametersHandler, IUnixClock clock)
        {
            Configuration = configuration;
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
            IDictionary<string, string> parameters = ParametersHandler.GetServerRequestParameters(Parameters);

            return new Uri(String.Format(
                "{0}://{1}:{2}/{3}/{4}?{5}",
                Configuration.RequestProtocol,
                Configuration.ServerAddress,
                Configuration.ServerPort,
                Configuration.Context,
                Action,
                parameters.ToUriQueryString()
            ));

        }

        public Uri GetUrlWithSimpleAuthentication()
        {
            IDictionary<string, string> parameters = ParametersHandler.GetServerRequestParameters(Parameters);

            parameters["timestamp"] = Clock.Now().ToString();
            parameters["username"] = Configuration.User;
            parameters["password"] = Configuration.Password.ToMD5();

            return new Uri(String.Format(
                "{0}://{1}:{2}/{3}/{4}?{5}",
                Configuration.RequestProtocol,
                Configuration.ServerAddress,
                Configuration.ServerPort,
                Configuration.Context,
                Action,
                parameters.ToUriQueryString()
            ));
        }

        public Uri GetUrlWithAdvancedAuthentication()
        {
            IDictionary<string, string> parameters = ParametersHandler.GetServerRequestParameters(Parameters);

            string timestamp = Clock.Now().ToString();

            parameters["timestamp"] = timestamp;
            parameters["username"] = Configuration.User;
            parameters["password"] = (
                Configuration.AdvancedAuthPrefix + 
                timestamp +
                Configuration.Password.ToMD5() +
                Configuration.AdvancedAuthPostfix
            ).ToMD5();

            return new Uri(String.Format(
                "{0}://{1}:{2}/{3}/{4}?{5}",
                Configuration.RequestProtocol,
                Configuration.ServerAddress,
                Configuration.ServerPort,
                Configuration.Context,
                Action,
                parameters.ToUriQueryString()
            ));
        }

        public Uri GetUrlWithHttpAuthentication()
        {
            IDictionary<string, string> parameters = ParametersHandler.GetServerRequestParameters(Parameters);

            return new Uri(String.Format(
                "{0}://{1}:{2}@{3}:{4}/{5}/{6}?{7}",
                Configuration.RequestProtocol,
                Configuration.User,
                Configuration.Password,
                Configuration.ServerAddress,
                Configuration.ServerPort,
                Configuration.Context,
                Action,
                parameters.ToUriQueryString()
            ));
        }
    }
}
