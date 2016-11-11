using System;
using System.Linq;
using Xunit;

namespace Restaurant.Tests
{
    public class MidgetTest
    {
        private readonly Guid _orderGuid = Guid.NewGuid();

        readonly Order _order = new Order
        {
            OrderId = "1",
            TableNumber = 5,
            ServerName = "Dave",
            TimeStamp = "12:00",
            TimeToCook = "00:00",
            Subtotal = 5.55M,
            Total = 6.66M,
            Tax = 1.11M,
            Ingredients = new[] { "Pasta", "Fish" },
            Paid = true,
            Items = new[]
                {
                    new OrderItem {ItemName = "5", Qty = 2, Price = 5.00M},
                    new OrderItem {ItemName = "6", Qty = 3, Price = 6.00M},
                }
        };

        private FakeBus bus;

        public MidgetTest()
        {
            bus = new FakeBus();
        }

        [Fact]
        public void WhenMidgetNotifiedOfOrderPlaced_ThenMidgetSendsCommandToCookFood()
        {
            var orderPlaced = new OrderPlaced(_order, Guid.NewGuid(), _orderGuid);

            var midget = new EnglishMidget(bus, _orderGuid);
            midget.Handle(orderPlaced);

            Assert.IsType<CookFood>(bus.Messages.Single());
        }

        [Fact]
        public void WhenMidgetNotifiedOfFoodCooked_ThenMidgetSendsCommandToPriceOrder()
        {
            var orderCooked = new OrderCooked(_order, Guid.NewGuid(), _orderGuid);

            var midget = new EnglishMidget(bus, _orderGuid);
            midget.Handle(orderCooked);

            Assert.IsType<PriceOrder>(bus.Messages.Single());
        }


        [Fact]
        public void WhenMidgetNotifiedOfOrderPriced_ThenMidgetSendsCommandToTakePayment()
        {
            var orderPriced = new OrderPriced(_order, Guid.NewGuid(), _orderGuid);

            var midget = new EnglishMidget(bus, _orderGuid);
            midget.Handle(orderPriced);

            Assert.IsType<TakePayment>(bus.Messages.Single());
        }
    }
}
