using System;
using System.Globalization;

namespace Restaurant.OrderHandlers
{
    public class Waiter
    {
        private readonly ITopicBasedPubSub _bus;
        private int _orderId = 1;

        public Waiter(ITopicBasedPubSub bus)
        {
            _bus = bus;
        }

        public string PlaceOrder(Guid correlationId)
        {
            DateTime liveUntil = DateTime.UtcNow.Add(TimeSpan.FromSeconds(10.0));
            var order = new Order
            {
                OrderId = _orderId++.ToString(CultureInfo.InvariantCulture),
                TableNumber = 5,
                ServerName = "Dave",
                TimeStamp = DateTime.Now.ToShortTimeString(),
                Items = new[]
                {
                    new OrderItem {ItemName = "Spaghetti Bolognese", Qty = 2},
                    new OrderItem {ItemName = "Fish", Qty = 3 },
                },
                LiveUntil = liveUntil
            };

            _bus.Publish(new OrderPlaced(order, Guid.Empty, correlationId, liveUntil));
            return order.OrderId;
        } 
    }
}
