using Task1.Command.Domain.Events.DataTypes;

namespace Task1.Command.Domain.Events;

public class StudentAddedEvent : Event<StudentAddedData>
{
    private StudentAddedEvent()
    {

    }
    public StudentAddedEvent(
         Guid aggregateId,
            string userId,
            StudentAddedData data,
            int version = 1
    ) : base(aggregateId, sequence: 1, userId, data, version)
    {

    }
}
