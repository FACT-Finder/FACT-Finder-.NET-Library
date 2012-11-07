using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Omikron.FactFinder
{
    public class UnixClockStub : IUnixClock
    {
        public long StubValue { get; set; }

        public UnixClockStub()
        {
            StubValue = 0;
        }

        public long Now()
        {
            return StubValue;
        }
    }
}
