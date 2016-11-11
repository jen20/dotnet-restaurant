using System.Collections.Generic;

namespace Restaurant.Tests
{
    public class FakeBus : IMessagePublisher
    {
        public readonly IList<IMessage> Messages = new List<IMessage>();

        public void Publish<T>(T message, string topic = null) where T : IMessage
        {
            Messages.Add(message);
        }
    }
}
