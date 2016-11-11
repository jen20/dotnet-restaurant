using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Restaurant.OrderHandlers
{
    public class QueuedHandler<T> : IHandle<T>, IStartable
    {
        private readonly ConcurrentQueue<T> _workQueue;
        private readonly IHandle<T> _handler;
        private readonly Thread _workerThread;

        public QueuedHandler(string name, IHandle<T> handler)
        {
            _handler = handler;
            _workQueue = new ConcurrentQueue<T>();

            _workerThread = new Thread(OrderHandler) { Name = name };
        }

        public decimal QueueCount { get { return _workQueue.Count;  } }

        private void OrderHandler()
        {
            while (true)
            {
                T message;
                if (_workQueue.TryDequeue(out message))
                    _handler.Handle(message);
                else
                    Thread.Sleep(1);
            }
        }

        public void Handle(T message)
        {
            _workQueue.Enqueue(message);
        }

        public void Start()
        {
            _workerThread.Start();
        }

        public string GetStatistics()
        {
            return string.Format("{0} queue count {1}", _workerThread.Name, _workQueue.Count);
        }       
    }
}
