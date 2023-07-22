using Task1.Command.Domain.Events.DataTypes;

namespace Task1.Command.Domain.Events;

public class StudentUpdatedEvent : Event<StudentUpdatedData>
{
    private StudentUpdatedEvent()
    {

    }
    public StudentUpdatedEvent(
         Guid aggregateId,
            string userId,
         int sequence,
            StudentUpdatedData data,
            int version = 1
    ) : base(aggregateId, sequence, userId, data, version)
    {

    }
}
