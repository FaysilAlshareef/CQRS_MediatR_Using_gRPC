using Task1.CQRS_MediatR_Using_gRPC.Enums;

namespace Task1.CQRS_MediatR_Using_gRPC.Events.DataTypes;

public record StudentUpdatedData
    : IEventData
{
    private StudentUpdatedData()
    {

    }
    public StudentUpdatedData(Guid studentId, string name, string phone_Number)
    {
        StudentId = studentId;
        Name = name;
        Phone_Number = phone_Number;
    }

    public Guid StudentId { get; private set; }
    public string Name { get; private set; }
    public string Phone_Number { get; private set; }
    public EventType Type => EventType.StudentUpdated;
}