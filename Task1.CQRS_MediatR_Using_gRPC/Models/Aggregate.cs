using Task1.CQRS_MediatR_Using_gRPC.Events;

namespace Task1.CQRS_MediatR_Using_gRPC.Models;

public abstract class Aggregate<T>
{
    private readonly List<Event> _uncommittedEvents = new();

    public Guid Id { get; protected set; }
    public int Sequence { get; internal set; }
    public IReadOnlyList<Event> GetUncommittedEvents() => _uncommittedEvents;
    public void MarkChangesAsCommitted() => _uncommittedEvents.Clear();

    public static T LoadFromHistory(List<Event> history)
    {
        if (history.Count == 0)
            throw new ArgumentOutOfRangeException(nameof(history), "history.Count == 0");

        var aggregate = (T)Activator.CreateInstance(typeof(T), nonPublic: true);

        foreach (var e in history)
        {
            ((dynamic)aggregate).ApplyChange(e, false);
        }

        return aggregate;
    }

    protected void ApplyChange(dynamic @event, bool isNew = true)
    {
        if (@event.Sequence == 1)
        {
            Id = @event.AggregateId;
        }

        Sequence++;

        if (Id == Guid.Empty)
            throw new InvalidOperationException("Id == Guid.Empty");

        if (@event.Sequence != Sequence)
            throw new InvalidOperationException("@event.Sequence != Sequence");

        ((dynamic)this).Apply(@event);

        if (isNew)
            _uncommittedEvents.Add(@event);
    }
}
