using System;
using System.Collections.Specialized;
using log4net;
using Omikron.FactFinder.Core.Client;
using Omikron.FactFinder.Core.Server;
using Omikron.FactFinder.Data;
using Omikron.FactFinder.Util;

namespace Omikron.FactFinder.Adapter
{
    public class Tracking : AbstractAdapter
    {
        private static ILog log;

        static Tracking()
        {
            log = LogManager.GetLogger(typeof(Tracking));
        }

        public Tracking(Request request, Core.Client.UrlBuilder urlBuilder)
            : base(request, urlBuilder)
        {
            log.Debug("Initialize new Tracking adapter.");

            Request.Action = RequestType.Tracking;
        }

        public bool DoTrackingFromRequest()
        {
            Parameters.Clear();
            Parameters.Add(new RequestParser().RequestParameters);

            return ApplyTracking();
        }

        public bool TrackEvent(EventType type, NameValueCollection inputParameters)
        {
            log.Debug(String.Format("Tracking \"{0}\" event.", type.ToString()));
            NameValueCollection parameters = PrepareDefaultParameters(inputParameters);
            parameters["event"] = type.ToString();
            Parameters.Clear();
            Parameters.Add(parameters);

            return ApplyTracking();
        }

        protected NameValueCollection PrepareDefaultParameters(NameValueCollection inputParameters)
        {
            var parameters = new NameValueCollection();

            string[] optionalParameters = new string[] {
                "userId",
                "cookieId"
            };
            foreach (var key in optionalParameters)
            {
                string value = inputParameters[key];
                if (!String.IsNullOrEmpty(value))
                    parameters[key] = value;
            }

            return parameters;
        }

        protected bool ApplyTracking()
        {
            return ((string)ResponseContent).Trim() == "The event was successfully tracked";
        }
    }
}
