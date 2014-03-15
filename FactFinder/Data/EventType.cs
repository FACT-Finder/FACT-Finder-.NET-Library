namespace Omikron.FactFinder.Data
{
    public sealed class EventType
    {
        private readonly string name;
        private readonly int value;

        public static readonly EventType Display = new EventType(1, "display");
        public static readonly EventType Feedback = new EventType(2, "feedback");
        public static readonly EventType Inspect = new EventType(3, "inspect");
        public static readonly EventType AvailabilityCheck = new EventType(4, "availabilityCheck");
        public static readonly EventType Cart = new EventType(5, "cart");
        public static readonly EventType Buy = new EventType(6, "buy");
        public static readonly EventType CacheHit = new EventType(7, "cacheHit");
        public static readonly EventType SessionStart = new EventType(8, "sessionStart");

        private EventType(int value, string name)
        {
            this.name = name;
            this.value = value;
        }

        public override string ToString()
        {
            return name;
        }
    }
}