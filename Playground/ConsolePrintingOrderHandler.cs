using System;
using Restaurant;
using Restaurant.OrderHandlers;

namespace Playground
{
    class ConsolePrintingOrderHandler : IHandle<OrderPaid>
    {
        private readonly ITopicBasedPubSub bus;

        public ConsolePrintingOrderHandler(ITopicBasedPubSub bus)
        {
            this.bus = bus;
        }

        public void Handle(OrderPaid message)
        {
            Console.WriteLine("Order Paid: {0}", message.CorrelationId);
        }
    }
}
