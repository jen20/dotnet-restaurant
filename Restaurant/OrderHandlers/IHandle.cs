namespace Restaurant.OrderHandlers
{
    public interface IHandler
    {
        //void Handle(IMessage message);
    }

    public interface IHandle<T> : IHandler
    {
        void Handle(T message);
    }
}