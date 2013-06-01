using System;
using System.Web;

namespace Omikron.FactFinder
{
    /// <summary>
    /// Abstraction of HttpContext.Current to make testing of the web service easier.
    /// Code mostly taken from http://stackoverflow.com/a/4053620/1633117.
    /// </summary>
    public class HttpContextFactory
    {
        private static HttpContextBase _context;

        /// <summary>
        /// Set this property to hand out a certain HttpContextBase object (e.g. a mock).
        /// If this is not set, the factory will attempt to supply HttpContext.Current.
        /// </summary>
        public static HttpContextBase Current
        {
            get
            {
                if (_context != null)
                {
                    return _context;
                }

                if (HttpContext.Current == null)
                {
                    throw new InvalidOperationException("HttpContext not available.");
                }

                return new HttpContextWrapper(HttpContext.Current);
            }

            set
            {
                _context = value;
            }
        }
    }
}
