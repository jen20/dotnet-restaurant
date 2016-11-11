using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using Restaurant.OrderHandlers;

namespace Playground
{
    public class Monitor
    {
        private readonly IEnumerable<IStartable> monitorables;

        public Monitor(IEnumerable<IStartable> monitorables)
        {
            this.monitorables = monitorables;
        }

        public void Start()
        {
            Task.Factory.StartNew(() =>
            {
                while (true)
                {
                    foreach (var monitorable in monitorables)
                    {
                        string message = monitorable.GetStatistics();
                        //Console.WriteLine(message);
                    }
                    Thread.Sleep(1000);
                }
            });
        }
    }
}