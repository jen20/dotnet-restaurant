using Restaurant.OrderHandlers;

namespace Restaurant
{
    public interface IMidget : IHandle<OrderPlaced>, IHandle<OrderCooked>, IHandle<OrderPriced>, IHandle<OrderPaid>
    {
    }
}