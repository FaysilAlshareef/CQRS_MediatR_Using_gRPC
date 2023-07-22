using Task1.Command.Domain.Commands;
using Task1.Command.Domain.Events;
using Task1.Command.Domain.Events.DataTypes;

namespace Task1.Command.Domain.Extensions;

public static class EventExtensions
{
    public static StudentAddedEvent ToEvent(this IStudentAddCommand command)
        => new(
            Guid.NewGuid(),
            "1",
            new StudentAddedData(
                command.Name,
                command.Address,
                command.Phone_Number
                )
            );

    public static StudentUpdatedEvent ToEvent(this IStudentUpdateCommand command, int sequence)
        => new(
            command.StudentId,
              "1",
              sequence,
              new StudentUpdatedData(
                  command.StudentId,
                  command.Name,
                  command.Phone_Number
                  )
            );
}
