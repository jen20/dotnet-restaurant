using System;

namespace Restaurant
{
    public class CookFood : BaseMessage
    {
        public CookFood(Order order, Guid causationId, Guid correlationId, DateTime? timeToLive = null)
            : base (causationId, correlationId, timeToLive)
        {
            Order = order;
        }

        public Order Order { get; set; }
    }

    public class PriceOrder : BaseMessage
    {
        public PriceOrder(Order order, Guid causationId, Guid correlationId, DateTime? timeToLive = null)
            : base(causationId, correlationId, timeToLive)
        {
            Order = order;
        }

        public Order Order { get; set; }
    }

    public class TakePayment : BaseMessage
    {
        public TakePayment(Order order, Guid causationId, Guid correlationId, DateTime? timeToLive = null)
            : base(causationId, correlationId, timeToLive)
        {
            Order = order;
        }

        public Order Order { get; set; }
    }
}
