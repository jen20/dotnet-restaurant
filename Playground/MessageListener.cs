using System;
using Restaurant;
using Restaurant.OrderHandlers;

namespace Playground
{
    public class MessageListener : IHandle<IMessage>
    {
        public MessageListener(ITopicBasedPubSub bus)
        {
            bus.Subscribe<IMessage>(this);
        }

        public void Handle(IMessage message)
        {
             Console.WriteLine("Message Id: {0}, Causation Id: {1}, Correlation Id: {2}, Message Type: {3}", message.MessageId, message.CausationId, message.CorrelationId, message.GetType().Name);
        }
    }
}
