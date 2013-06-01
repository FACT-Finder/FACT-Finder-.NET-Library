using log4net;

namespace Omikron.FactFinder
{
    public class EncodingHandler
    {
        private static ILog log;

        static EncodingHandler()
        {
            log = LogManager.GetLogger(typeof(EncodingHandler));
        }

        public EncodingHandler()
        {
        }
    }
}
