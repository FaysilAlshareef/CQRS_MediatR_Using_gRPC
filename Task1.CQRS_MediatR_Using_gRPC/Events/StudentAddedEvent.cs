using Task1.CQRS_MediatR_Using_gRPC.Events.DataTypes;

namespace Task1.CQRS_MediatR_Using_gRPC.Events;

public class StudentAddedEvent : Event<StudentAddedData>
{
    protected StudentAddedEvent()
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
