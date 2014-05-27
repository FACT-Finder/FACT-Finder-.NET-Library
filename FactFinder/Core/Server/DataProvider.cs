using System.Collections.Generic;
using log4net;

namespace Omikron.FactFinder.Core.Server
{
    public abstract class DataProvider
    {
        protected Dictionary<int, ConnectionData> ConnectionData;

        static private int NextID = 0;

        private static ILog log;

        static DataProvider()
        {
            log = LogManager.GetLogger(typeof(DataProvider));
        }

        public DataProvider()
        {
            ConnectionData = new Dictionary<int, ConnectionData>();
        }

        /**
         * Make a connection data object known to the data provider and obtain an ID
         * for it (basically, a handle).
         */
        public int Register(ConnectionData connectionData)
        {
            int id = NextID++;
            ConnectionData[id] = connectionData;

            log.DebugFormat("Registered connection data for ID {0}.", id);

            return id;
        }
        
        /**
         * Remove all references to the connection data object identified by id.
         */
        public void Unregister(int id)
        {
            ConnectionData.Remove(id);

            log.DebugFormat("Unregistered connection data for ID {0}.", id);
        }

        /**
         * Load a response based on the current state of the connection data
         * corresponding to id and fill that ConnectionData object with this
         * response.
         * 
         * Note: The response is NOT returned by this function. It has to be
         *       obtained directly from the ConnectionData object.
         */
        abstract public void LoadResponse(int id);
    }
}
