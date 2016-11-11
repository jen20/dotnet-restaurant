using System;

namespace Restaurant
{
    public class BaseMessage : IMessage
    {
        public BaseMessage(Guid causationId, Guid correlationId, DateTime? timeToLive = null)
        {
            CausationId = causationId;
            CorrelationId = correlationId;
            TimeToLive = timeToLive;
            MessageId = Guid.NewGuid();
        }

        public Guid MessageId { get; private set; }
        public Guid CausationId { get; private set; }
        public Guid CorrelationId { get; private set; }
        public DateTime? TimeToLive { get; set; }
    }

    public class OrderPlaced : BaseMessage
    {
        public OrderPlaced(Order order, Guid causationId, Guid correlationId, DateTime? timeToLive = null)
            : base(causationId, correlationId, timeToLive)
        {
            Order = order;
        }
        public Order Order { get; private set; }
    }

    public class OrderCooked : BaseMessage
    {
        public OrderCooked(Order order, Guid causationId, Guid correlationId, DateTime? timeToLive = null)
            : base(causationId, correlationId, timeToLive)
        {
            Order = order;
        }
        public Order Order { get; private set; }
    }

    public class OrderPriced : BaseMessage
    {
        public OrderPriced(Order order, Guid causationId, Guid correlationId, DateTime? timeToLive = null)
            : base(causationId, correlationId, timeToLive)
        {
            Order = order;
        }

        public Order Order { get; private set; }
    }

    public class OrderPaid : BaseMessage
    {
        public OrderPaid(Order order, Guid causationId, Guid correlationId, DateTime? timeToLive = null)
            : base(causationId, correlationId, timeToLive)
        {
            Order = order;
        }

        public Order Order { get; private set; }
    }

    public class OrderComplete : BaseMessage
    {
        public OrderComplete(Order order, Guid causationId, Guid correlationId, DateTime? timeToLive = null)
            : base(causationId, correlationId, timeToLive)
        {
            Order = order;
        }

        public Order Order { get; private set; }
    }

}
