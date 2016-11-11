using System;

namespace Restaurant
{
    public interface IMessage
    {
        Guid MessageId { get; }
        Guid CausationId { get; }
        Guid CorrelationId { get; }
        DateTime? TimeToLive { get; }
    }
}