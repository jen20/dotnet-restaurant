using System;

namespace Restaurant
{
    public class ZimbabweanMidget : IMidget
    {
        private readonly IMessagePublisher _bus;

        public ZimbabweanMidget(IMessagePublisher bus, Guid orderId)
        {
            _bus = bus;
            OrderId = orderId;
        }

        public Guid OrderId { get; }

        public void Handle(OrderPlaced message)
        {
            _bus.Publish(new PriceOrder(message.Order, message.MessageId, message.CorrelationId, DateTime.UtcNow.AddSeconds(10)));
        }

        public void Handle(OrderPriced message)
        {
            _bus.Publish(new TakePayment(message.Order, message.MessageId, message.CorrelationId));
        }

        public void Handle(OrderPaid message)
        {
            _bus.Publish(new CookFood(message.Order, message.MessageId, message.CorrelationId));
        }

        public void Handle(OrderCooked message)
        {
            _bus.Publish(new OrderComplete(message.Order, message.MessageId, message.CorrelationId));
        }
    }
}