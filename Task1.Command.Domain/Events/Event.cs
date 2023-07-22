


using Task1.Command.Domain.Enums;
using Task1.Command.Domain.Events.DataTypes;

namespace Task1.Command.Domain.Events;

public abstract class Event
{
    protected Event() { }

    public long Id { get; protected set; }
    public Guid AggregateId { get; protected set; }
    public int Sequence { get; protected set; }
    public string UserId { get; protected set; }
    public EventType Type { get; protected set; }
    public DateTime DateTime { get; protected set; }
    public int Version { get; protected set; }
}

public abstract class Event<TData> : Event
    where TData : IEventData
{
    protected Event() { }
    protected Event(
        Guid aggregateId,
        int sequence,
        string userId,
        TData data,
        int version = 1
    )
    {
        AggregateId = aggregateId;
        Sequence = sequence;
        UserId = userId;
        Type = data.Type;
        Data = data;
        DateTime = DateTime.UtcNow;
        Version = version;
    }

    public TData Data { get; protected set; }
}
