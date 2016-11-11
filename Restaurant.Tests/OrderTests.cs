using Newtonsoft.Json.Linq;
using Xunit;

namespace Restaurant.Tests
{
    public class OrderTests
    {
        [Fact]
        public void Serialise()
        {
            var order = new Order
            {
                OrderId = "1",
                TableNumber = 5,
                ServerName = "Dave",
                TimeStamp = "12:00",
                TimeToCook = "00:00",
                Subtotal = 5.55M,
                Total = 6.66M,
                Tax = 1.11M,
                Ingredients = new[] {"Pasta", "Fish"},
                Paid = true,
                Items = new[]
                {
                    new OrderItem {ItemName = "5", Qty = 2, Price = 5.00M},
                    new OrderItem {ItemName = "6", Qty = 3, Price = 6.00M},
                }
            };

            var json = order.Serialize();

            Assert.Equal(json.GetValue("OrderId").ToString(), "1");
            Assert.Equal(json.GetValue("TableNumber").ToObject<int>(), 5);
            Assert.Equal(json.GetValue("ServerName").ToString(), "Dave");
            Assert.Equal(json.GetValue("TimeStamp").ToString(), "12:00");
            Assert.Equal(json.GetValue("TimeToCook").ToString(), "00:00");
            Assert.Equal(json.GetValue("Subtotal").ToObject<decimal>(), 5.55M);
            Assert.Equal(json.GetValue("Total").ToObject<decimal>(), 6.66M);
            Assert.Equal(json.GetValue("Tax").ToObject<decimal>(), 1.11M);
            Assert.Equal(json.GetValue("Paid").ToObject<bool>(), true);

            var ingredients = json.GetValue("Ingredients");
            Assert.Equal(ingredients[0].ToString(), "Pasta");
            Assert.Equal(ingredients[1].ToString(), "Fish");

            var items = json.GetValue("Items");
            var item1 = items[0];
            Assert.Equal(item1["ItemName"].ToObject<string>(), "5");
            Assert.Equal(item1["Qty"].ToObject<int>(), 2);
            Assert.Equal(item1["Price"].ToObject<decimal>(), 5.00M);
            var item2 = items[1];
            Assert.Equal(item2["ItemName"].ToObject<string>(), "6");
            Assert.Equal(item2["Qty"].ToObject<int>(), 3);
            Assert.Equal(item2["Price"].ToObject<decimal>(), 6.00M);
        }

        [Fact]
        public void Deserialise()
        {
            var doc = new JObject
            {
                {"OrderId", "1"},
                {"TableNumber", 5},
                {"ServerName", "Dave"},
                {"TimeStamp", "12:00"},
                {"TimeToCook", "00:00"},
                {"Subtotal", 5.55M},
                {"Total", 6.66M},
                {"Tax", 1.11M},
                {"Ingredients", new JArray(new [] { (object) "Pasta", "Fish"})},
                {"Items", new JArray(new JObject { {"ItemName", 5}, {"Qty", 2}, {"Price", 5M}},new JObject { {"ItemName", 6}, {"Qty", 1}, {"Price", 6M} })},
                {"Paid", true}
            };

            var order = Order.Deserialise(doc.ToString());

            Assert.Equal(order.OrderId, "1");
            Assert.Equal(order.TableNumber, 5);
            Assert.Equal(order.ServerName, "Dave");
            Assert.Equal(order.TimeStamp, "12:00");
            Assert.Equal(order.TimeToCook, "00:00");
            Assert.Equal(order.Subtotal, 5.55M);
            Assert.Equal(order.Total, 6.66M);
            Assert.Equal(order.Tax, 1.11M);
            Assert.True(order.Paid);

            Assert.Equal(order.Ingredients[0], "Pasta");
            Assert.Equal(order.Ingredients[1], "Fish");

            var item1 = order.Items[0];
            Assert.Equal(item1.ItemName, "5");
            Assert.Equal(item1.Qty, 2);
            Assert.Equal(item1.Price, 5.00M);
            var item2 = order.Items[1];
            Assert.Equal(item2.ItemName, "6");
            Assert.Equal(item2.Qty, 1);
            Assert.Equal(item2.Price, 6.00M);
        }

        [Fact]
        public void PreservesRandomCrap()
        {
            var doc = new JObject
            {
                {"OrderId", "1"},
                {"TableNumber", 5},
                {"ServerName", "Dave"},
                {"TimeStamp", "12:00"},
                {"TimeToCook", "00:00"},
                {"Subtotal", 5.55M},
                {"Total", 6.66M},
                {"Tax", 1.11M},
                {"Ingredients", new JArray(new [] { (object) "Pasta", "Fish"})},
                {"Items", new JArray(new JObject { {"ItemName", 5}, {"Qty", 2}, {"Price", 5M}},new JObject { {"ItemName", 6}, {"Qty", 1}, {"Price", 6M} })},
                {"Random Crap", "Tear in page"},
                {"Paid", true}
            };

            var order = Order.Deserialise(doc.ToString());
            var json = order.Serialize();

            Assert.Equal(json.GetValue("Random Crap").ToObject<string>(), "Tear in page");
        }
    }
}
