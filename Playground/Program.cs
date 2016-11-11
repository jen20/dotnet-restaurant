using System;
using System.Collections.Generic;
using Restaurant;
using Restaurant.OrderHandlers;

namespace Playground
{
    class Program
    {
        private const int NumberOfChefs = 3;

        private const int NumberOfOrders = 1;

        static void Main()
        {
            var bus = new TopicBasedPubSub();
            var messageListener = new MessageListener(bus);
            var startables = new List<IStartable>();
            
            var midgetFactory = new MidgetFactory();
            var midgetHouse = new MidgetHouse(bus, midgetFactory);

            var consolePrinter = new QueuedHandler<OrderPaid>(Messages.Paid, new ConsolePrintingOrderHandler(bus));
            bus.Subscribe(consolePrinter);

            startables.Add(consolePrinter);
            var cashier = new Cashier(bus);
            var queuedCashier = new QueuedHandler<TakePayment>(Messages.OrderBilled, cashier);
            bus.Subscribe(queuedCashier);
            startables.Add(queuedCashier);
            startables.Add(cashier);

            var assistantManager = new QueuedHandler<PriceOrder>(Messages.OrderPrepared, new AssistantManager(bus));
            bus.Subscribe(assistantManager);
            startables.Add(assistantManager);
          
            var chefs = new List<QueuedHandler<CookFood>>();
            var rand = new Random();
            for (int i = 0; i < NumberOfChefs; i++)
            {
                var chef = new TimeToLiveDispatcher<CookFood>(new Chef(bus, rand.Next(1000)));
                var queuedHandler = new QueuedHandler<CookFood>(string.Format("Chef {0}", i), chef);
                chefs.Add(queuedHandler);
                startables.Add(queuedHandler);
            }

            var distributionStrategy = new QueuedDispatcher<CookFood>(bus, chefs);
            startables.Add(distributionStrategy);

            foreach (var startable in startables)
            {
                startable.Start();
            }
            var monitor = new Monitor(startables);
            monitor.Start();

            var waiter = new Waiter(bus);
            
            for (int i = 0; i < NumberOfOrders; i++)
            {
                var correlationId = Guid.NewGuid();

                var orderId = waiter.PlaceOrder(correlationId);
                Console.WriteLine("Started order number: {0}...", orderId);
            }

            Console.ReadKey();
        }
    }
}
