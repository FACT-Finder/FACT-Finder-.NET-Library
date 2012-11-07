using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omikron.FactFinder
{
    public interface IUnixClock
    {
        /**
         * Returns the total number of milliseconds since 1970/01/01 00:00
         **/
        long Now();
    }
}
