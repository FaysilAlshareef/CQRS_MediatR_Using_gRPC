using Task1.CQRS_MediatR_Using_gRPC.Commands;
using Task1.CQRS_MediatR_Using_gRPC.Events;
using Task1.CQRS_MediatR_Using_gRPC.Events.DataTypes;

namespace Task1.CQRS_MediatR_Using_gRPC.Extensions;

public static class EventExtensions
{
    public static StudentAddedEvent ToEvent(this StudentAddCommand command)
        => new(
            Guid.NewGuid(),
            "1",
            new StudentAddedData(
                command.Name,
                command.Address,
                command.Phone_Number
                )
            );
}
