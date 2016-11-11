using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

namespace Restaurant.OrderHandlers
{
    public class QueuedDispatcher<T> : IStartable, IHandle<T> where T : IMessage
    {
        private readonly ConcurrentQueue<T>  _outerQueue;
        private readonly IEnumerable<QueuedHandler<T>> _childHandlers;

        public QueuedDispatcher(ITopicBasedPubSub bus, IEnumerable<QueuedHandler<T>> childHandlers)
        {
            bus.Subscribe(this);
            _childHandlers = childHandlers;
            _outerQueue = new ConcurrentQueue<T>();
        }

        public void Start()
        {
            Task.Factory.StartNew(StartProcessingOrders, TaskCreationOptions.LongRunning);
        }

        private void StartProcessingOrders()
        {
            while (true)
            {
                T message;
                while (_outerQueue.TryDequeue(out message))
                {
                    bool pending = true;
                    while (pending)
                    {
                        foreach (var childHandler in _childHandlers)
                        {
                            if (childHandler.QueueCount < 5)
                            {
                                try
                                {
                                    childHandler.Handle(message);
                                    pending = false;
                                    break;
                                }
                                catch (Exception ex)
                                {
                                    //TODO: Handle poison messages
                                    Console.WriteLine("Error handling order: {0}", ex);
                                }
                           }
                        }
                        Thread.Sleep(1);
                    }
                }
                Thread.Sleep(1);
            }
        }

        public string GetStatistics()
        {
            return string.Format("Outer queue length: {0}", _outerQueue.Count);
        }

        public void Handle(T message)
        {
            _outerQueue.Enqueue(message);
        }
    }
}
