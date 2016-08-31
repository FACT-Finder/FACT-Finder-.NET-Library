namespace Omikron.FactFinder.Data
{
    public sealed class EventType
    {
        private readonly string name;
        private readonly int value;

        public static readonly EventType Click = new EventType(1, "click");
        public static readonly EventType Cart = new EventType(2, "cart");
        public static readonly EventType Checkout = new EventType(3, "checkout");
        public static readonly EventType RecommendationClick = new EventType(4, "recommendationClick");
        public static readonly EventType Login = new EventType(5, "login");

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