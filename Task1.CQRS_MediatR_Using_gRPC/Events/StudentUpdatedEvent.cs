using Task1.CQRS_MediatR_Using_gRPC.Events.DataTypes;

namespace Task1.CQRS_MediatR_Using_gRPC.Events;

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
