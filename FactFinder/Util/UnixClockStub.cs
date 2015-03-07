
namespace Omikron.FactFinder.Util
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
