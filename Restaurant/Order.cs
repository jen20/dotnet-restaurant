using System;
using System.Linq;
using Newtonsoft.Json.Linq;

namespace Restaurant
{
    public class Order : IHaveTTL
    {
        public string OrderId { get; set; }
        public int TableNumber { get; set; }
        public string ServerName { get; set; }
        public string TimeStamp { get; set; }
        public string TimeToCook { get; set; }
        public decimal Subtotal { get; set; }
        public decimal Total { get; set; }
        public decimal Tax { get; set; }
        public string[] Ingredients { get; set; }
        public OrderItem[] Items { get; set; }

        public JObject OriginalDoc { get; set; }
        public bool Paid { get; set; }

        public JObject Serialize()
        {
            var doc = OriginalDoc ?? new JObject();

            doc["OrderId"] = OrderId;
            doc["TableNumber"] = TableNumber;
            doc["ServerName"] = ServerName;
            doc["TimeStamp"] = TimeStamp;
            doc["TimeToCook"] = TimeToCook;
            doc["Subtotal"] = Subtotal;
            doc["Total"] = Total;
            doc["Tax"] = Tax;
            doc["Ingredients"] = new JArray(Ingredients.Cast<object>());
            doc["Items"] = new JArray(Items.Select(i => new JObject { {"ItemName", i.ItemName}, {"Qty", i.Qty}, {"Price", i.Price}}));
            doc["Paid"] = Paid;

            return doc;
        }

        public static Order Deserialise(string json)
        {
            var jsonObject = JObject.Parse(json);

            var order = new Order();
            order.OrderId = jsonObject.GetValue("OrderId").ToObject<string>();
            order.TableNumber = jsonObject.GetValue("TableNumber").ToObject<int>();
            order.ServerName = jsonObject.GetValue("ServerName").ToObject<string>();
            order.TimeStamp = jsonObject.GetValue("TimeStamp").ToObject<string>();
            order.TimeToCook = jsonObject.GetValue("TimeToCook").ToObject<string>();
            order.Subtotal = jsonObject.GetValue("Subtotal").ToObject<decimal>();
            order.Total = jsonObject.GetValue("Total").ToObject<decimal>();
            order.Tax = jsonObject.GetValue("Tax").ToObject<decimal>();
            order.Ingredients = jsonObject.GetValue("Ingredients").Select(i => i.ToObject<string>()).ToArray();
            order.Items = jsonObject.GetValue("Items").Select(i => 
                new OrderItem
                {
                    ItemName = i["ItemName"].ToObject<string>(), 
                    Qty = i["Qty"].ToObject<int>(), 
                    Price = i["Price"].ToObject<decimal>()
                }).ToArray();
            order.OriginalDoc = jsonObject;
            order.Paid = jsonObject.GetValue("Paid").ToObject<bool>();
            return order;
        }

        public DateTime LiveUntil { get; set; }
    }

    public interface IHaveTTL
    {
        DateTime LiveUntil { get; }
    }
}