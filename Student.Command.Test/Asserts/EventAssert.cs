using Students.Command.Test.Protos;
using Task1.Command.Domain.Entities;
using Task1.Command.Domain.Enums;
using Task1.Command.Domain.Events;

namespace Students.Command.Test.Asserts;
public static class EventAssert
{
    public static void AssertEquality(
        CreateRequest createRequest,
        StudentOutput studentOutput,
        Event studentEvent
        )
    {
        Assert.NotNull(createRequest);
        Assert.NotNull(studentEvent);
        Assert.NotNull(studentOutput);

        // Request Vs Output 
        Assert.Equal(createRequest.Name, studentOutput.Name);
        Assert.Equal(createRequest.Address, studentOutput.Address);
        Assert.Equal(createRequest.PhoneNumber, studentOutput.PhoneNumber);

        // Output vs Event
        Assert.Equal(studentOutput.Id, studentEvent.AggregateId.ToString());
        Assert.Equal(1, studentEvent.Sequence);
        Assert.Equal(1, studentEvent.Version);
        Assert.Equal(EventType.StudentAdded, studentEvent.Type);
        Assert.Equal(DateTime.UtcNow, studentEvent.DateTime, TimeSpan.FromMinutes(1));


        var @event = (StudentAddedEvent)studentEvent;

        var data = @event.Data;
        //Output Vs Event Data
        Assert.Equal(studentOutput.Name, data.Name);
        Assert.Equal(studentOutput.Address, data.Address);
        Assert.Equal(studentOutput.PhoneNumber, data.Phone_Number);

    }

    public static void AssertEquality(
        Event studentEvent,
        OutboxMessage message
        )
    {
        Assert.NotNull(studentEvent);
        Assert.NotNull(message);

        Assert.Equal(studentEvent.Sequence, message.Event.Sequence);
        Assert.Equal(1, message.Event.Version);
        Assert.Equal(studentEvent.Type, message.Event.Type);
        Assert.Equal(studentEvent.DateTime, message.Event.DateTime, TimeSpan.FromMinutes(1));

        Assert.Equal(((StudentAddedEvent)studentEvent).Data, ((StudentAddedEvent)message.Event).Data);
        Assert.Equal(studentEvent.Id, message.Event.Id);



    }

    public static void AssertEquality(
    UpdateStudentRequest updateStudent,
    Event studentEvent,
    int sequence
    )
    {
        Assert.NotNull(updateStudent);
        Assert.NotNull(studentEvent);


        Assert.Equal(updateStudent.StudentId, studentEvent.AggregateId.ToString());
        Assert.Equal(EventType.StudentUpdated, studentEvent.Type);
        Assert.Equal(sequence, studentEvent.Sequence);
        Assert.Equal(1, studentEvent.Version);
        Assert.Equal(DateTime.UtcNow, studentEvent.DateTime, TimeSpan.FromMinutes(1));


        var @event = (StudentUpdatedEvent)studentEvent;

        var data = @event.Data;
        Assert.Equal(updateStudent.Name, data.Name);
        Assert.Equal(updateStudent.PhoneNumber, data.Phone_Number);
    }

    public static void AssertEquality(
            OutboxMessage message,
            Event studentUpdated
            )
    {
        Assert.NotNull(studentUpdated);
        Assert.NotNull(message);

        Assert.Equal(studentUpdated.Sequence, message.Event.Sequence);
        Assert.Equal(1, message.Event.Version);
        Assert.Equal(studentUpdated.Type, message.Event.Type);
        Assert.Equal(studentUpdated.DateTime, message.Event.DateTime, TimeSpan.FromMinutes(1));

        Assert.Equal(((StudentUpdatedEvent)studentUpdated).Data, ((StudentUpdatedEvent)message.Event).Data);
        Assert.Equal(studentUpdated.Id, message.Event.Id);
    }
}
