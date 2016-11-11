using System.Collections.Generic;
using System.Linq;
using System.Threading;
using Restaurant.OrderHandlers;

namespace Restaurant
{
    public class Chef : IHandle<CookFood>
    {
        private readonly ITopicBasedPubSub _bus;
        private readonly int _timeToCook;

        public Chef(ITopicBasedPubSub bus, int timeToCook)
        {
            _bus = bus;
            _timeToCook = timeToCook;
        }

        private readonly IDictionary<string, string> ingredientDb = new Dictionary<string, string>()
        {
            {"Spaghetti Bolognese", "Pasta, Tomatoes, Mince"},
            {"Fish", "Cod, Chips, Mushy Peas"},
        };

        public void Handle(CookFood message)
        {
            Thread.Sleep(_timeToCook);
            var order = message.Order;
            order.Ingredients = order.Items.Select(i => ingredientDb[i.ItemName]).ToArray();

            _bus.Publish(new OrderCooked(order, message.MessageId, message.CorrelationId));
        }
    }
}
