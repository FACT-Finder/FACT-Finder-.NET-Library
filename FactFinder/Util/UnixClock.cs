using System;

namespace Omikron.FactFinder.Util
{
    public class UnixClock : IUnixClock
    {
        public long Now()
        {
            TimeSpan timeSpan = (DateTime.UtcNow - new DateTime(1970, 1, 1, 0, 0, 0));
            return (long)timeSpan.TotalMilliseconds;
        }
    }
}