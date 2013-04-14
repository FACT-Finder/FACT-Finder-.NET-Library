using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using log4net;

namespace Omikron.FactFinder
{
    public abstract class DataProvider
    {
        public virtual NameValueCollection Parameters { get; protected set; }
        public virtual RequestType Type { get; set; }

        public virtual string Data { get; private set; }

        private static ILog log;

        static DataProvider()
        {
            log = LogManager.GetLogger(typeof(DataProvider));
        }

        public DataProvider()
        {
            Parameters = new NameValueCollection();
        }

        /// <summary>
        /// Sets the given parameters. All previous values for the given keys will be replaced.
        /// Unmentioned keys will remain.
        /// </summary>
        /// <param name="parameters">Key-value pairs to be added.</param>
        public virtual void SetParameters(NameValueCollection parameters)
        {
            foreach (string key in parameters)
            {
                Parameters.Remove(key);
            }

            Parameters.Add(parameters);
        }

        public virtual void ResetParameters(NameValueCollection parameters)
        {
            Parameters = parameters;
        }

        public virtual void SetParameter(KeyValuePair<string, string> parameter)
        {
            Parameters[parameter.Key] = parameter.Value;
        }

        public virtual void SetParameter(string name, string value)
        {
            Parameters[name] = value;
        }

        public virtual void UnsetParameter(string name)
        {
            Parameters.Remove(name);
        }
    }
}
