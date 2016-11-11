using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using Restaurant.OrderHandlers;

namespace Restaurant
{
    public class TopicBasedPubSub : ITopicBasedPubSub
    {
        private readonly IDictionary<string, IMultiplexer> subscriptions = new ConcurrentDictionary<string, IMultiplexer>(); 

        public void Subscribe<T>(IHandle<T> handler, string topic = null) where T : IMessage
        {
           // Console.WriteLine("Subscribing to {0}", typeof(T).Name);
            IMultiplexer multiplexer;

            topic = topic ?? typeof (T).Name;
            if (!subscriptions.TryGetValue(topic, out multiplexer))
            {
                multiplexer = new Multiplexer();
                subscriptions.Add(topic, multiplexer);
            }
            ((Multiplexer) multiplexer).Add(handler);
        }

        public void SusbcribeOnCorrelationId(IHandle<IMessage> handler, Guid correlationId)
        {
            Subscribe(handler, correlationId.ToString());
        }

        public void UnsubscribeOnCorrelationId(Guid correlationId)
        {
            subscriptions.Remove(correlationId.ToString());
        }

        public void Publish<T>(T message, string topic = null)
            where T : IMessage
        {
        //    Console.WriteLine("Publishing to {0}", typeof(T).Name);
            topic = topic ?? typeof(T).Name;
            IMultiplexer multiplexer;
            if (subscriptions.TryGetValue(topic, out multiplexer))
            {
                var typedMultiplexer = (Multiplexer)  multiplexer;
                typedMultiplexer.Handle(message);
            }

            IMultiplexer messageMultiplexer;
            if (subscriptions.TryGetValue("IMessage", out messageMultiplexer))
            {
                var typedMultiplexer = (Multiplexer) messageMultiplexer;
                typedMultiplexer.Handle(message);
            }

            // publish to subscribers subscribed on correlationId
            IMultiplexer correlationIdMultiplexer;
            if (subscriptions.TryGetValue(message.CorrelationId.ToString(), out correlationIdMultiplexer))
            {
                var typedMultiplexer = (Multiplexer) correlationIdMultiplexer;
                typedMultiplexer.Handle(message);
            }
        }
    }

    public interface IMessagePublisher
    {
        void Publish<T>(T message, string topic = null) where T : IMessage;
    }

    public interface ITopicBasedPubSub : IMessagePublisher
    {
        void Subscribe<T>(IHandle<T> handler, string topic = null) where T : IMessage;
        void SusbcribeOnCorrelationId(IHandle<IMessage> handler, Guid correlationId);
        void UnsubscribeOnCorrelationId(Guid correlationId);
    }
}
