using Task1.CQRS_MediatR_Using_gRPC.Enums;

namespace Task1.CQRS_MediatR_Using_gRPC.Events.DataTypes;

public record StudentAddedData
    (
       string Name,
    string Address,
    string Phone_Number
    ) : IEventData
{
    public EventType Type => EventType.StudentAdded;
}
