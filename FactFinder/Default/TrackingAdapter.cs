using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using log4net;
using Omikron.FactFinder.Data;

namespace Omikron.FactFinder.Default
{
    public class TrackingAdapter : Adapter
    {
        private static ILog log;

        static TrackingAdapter()
        {
            log = LogManager.GetLogger(typeof(TrackingAdapter));
        }

        public TrackingAdapter(DataProvider dataProvider, ParametersHandler parametersHandler)
            : base(dataProvider, parametersHandler)
        {
            log.Debug("Initialize new TrackingAdapter.");
        }

        public bool DoTrackingFromRequest()
        {
            DataProvider.ResetParameters(ParametersHandler.GetRequestParamsForServer());

            return ApplyTracking();
        }

        public bool TrackEvent(EventType type, NameValueCollection inputParameters)
        {
            log.Debug(String.Format("Tracking \"{0}\" event.", type.ToString()));
            NameValueCollection parameters = PrepareDefaultParameters(inputParameters);
            parameters["event"] = type.ToString();
            DataProvider.ResetParameters(parameters);

            return ApplyTracking();
        }

        protected NameValueCollection PrepareDefaultParameters(NameValueCollection inputParameters)
        {
            string sid = inputParameters["sid"];

            if (String.IsNullOrEmpty(sid))
                sid = HttpContextFactory.Current.Session.SessionID;

            string sourceRefKey = inputParameters["sourceRefKey"];
            if (String.IsNullOrEmpty(sourceRefKey))
                throw new ArgumentException("Parameters need to contain sourceRefKey.");

            var parameters = new NameValueCollection();
            parameters["sid"] = sid;
            parameters["sourceRefKey"] = sourceRefKey;

            string[] optionalParameters = new string[] {
                "userId",
                "cookieId",
                "price",
                "amount",
                "positive",
                "message"
            };
            foreach (var key in optionalParameters)
            {
                string value = inputParameters[key];
                if (!String.IsNullOrEmpty(value))
                    parameters[key] = value;
            }

            return parameters;
        }
        
        protected virtual bool ApplyTracking()
        {
            log.Error("Tracking not available before FF 6.9!");
            return false;
        }
    }
}
