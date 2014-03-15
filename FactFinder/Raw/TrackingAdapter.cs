using log4net;
using Omikron.FactFinder.Core.Server;
using Omikron.FactFinder.Default;

namespace Omikron.FactFinder.Raw
{
    public class RawTrackingAdapter : TrackingAdapter
    {
        private static ILog log;

        static RawTrackingAdapter()
        {
            log = LogManager.GetLogger(typeof(RawTrackingAdapter));
        }

        public RawTrackingAdapter(DataProvider dataProvider, ParametersHandler parametersHandler)
            : base(dataProvider, parametersHandler)
        {
            DataProvider.Type = RequestType.Tracking;
        }

        protected override bool ApplyTracking()
        {
            return Data.Trim() == "The event was successfully tracked";
        }
    }
}
