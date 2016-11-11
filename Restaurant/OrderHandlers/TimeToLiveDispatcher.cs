using System;

namespace Restaurant.OrderHandlers
{
    public class TimeToLiveDispatcher<T> : IHandle<T> where T : IMessage
    {
        private readonly IHandle<T> _handler;

        public TimeToLiveDispatcher(IHandle<T> handler)
        {
            _handler = handler;
        }

        public void Handle(T message)
        {
            if (!message.TimeToLive.HasValue || message.TimeToLive > DateTime.UtcNow)
            {
                _handler.Handle(message);
            } else {
                Console.WriteLine("Message past TTL, discarding. {0}", message.MessageId);
            }
        }
    }
}
