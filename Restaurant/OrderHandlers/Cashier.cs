using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;

namespace Restaurant.OrderHandlers
{
    public class Cashier : IHandle<TakePayment>, IStartable
    {
        private readonly ITopicBasedPubSub _bus;
        private readonly ConcurrentQueue<TakePayment> _orders = new ConcurrentQueue<TakePayment>();

        public Cashier(ITopicBasedPubSub bus)
        {
            _bus = bus;
        }

        public void Handle(TakePayment message)
        {
            _orders.Enqueue(message);
        }

        public void HandleOutstandingPayments()
        {
            while (true)
            {
                TakePayment takePayment;
                while (_orders.TryDequeue(out takePayment))
                {
                    takePayment.Order.Paid = true;
                    _bus.Publish(new OrderPaid(takePayment.Order, takePayment.MessageId, takePayment.CorrelationId));
                }
                Thread.Sleep(1);
            }       
        }

        public void Start()
        {
            Task.Factory.StartNew(HandleOutstandingPayments, TaskCreationOptions.LongRunning);
        }

        public string GetStatistics()
        {
            return string.Format("Cashier queue count {0}", _orders.Count);
        }

        
    }
}
