using System;
using System.Collections.Generic;

namespace Restaurant.OrderHandlers
{
    public class Multiplexer : IHandler, IMultiplexer
    {
        private readonly IDictionary<Type, List<IHandler>> _handlers;

        public Multiplexer()
        {
            _handlers = new Dictionary<Type, List<IHandler>>();
        }

        public void Add<T>(IHandle<T> handler)
        {
            List<IHandler> existingHandlers;
            if (!_handlers.TryGetValue(typeof(T), out existingHandlers))
            {
                existingHandlers = new List<IHandler>();
                _handlers.Add(typeof(T), existingHandlers);
            }
            existingHandlers.Add(handler);
        }

        public void Handle<T>(T msg) where T : IMessage
        {
            List<IHandler> messageHandlers;
            if (_handlers.TryGetValue(typeof(T), out messageHandlers))
            {
                foreach (var handler in messageHandlers)
                {
                    var typedHandler = (IHandle<T>) handler;
                    typedHandler.Handle(msg);                    
                }
            }

            // publish to any handlers which subscribe to IMessage
            List<IHandler> allMessageHandlers;
            if (_handlers.TryGetValue(typeof (IMessage), out allMessageHandlers))
            {
                foreach (var handler in allMessageHandlers)
                {
                    var typedHandler = (IHandle<IMessage>) handler;
                    typedHandler.Handle(msg);
                }
            }
        }
    }
}
