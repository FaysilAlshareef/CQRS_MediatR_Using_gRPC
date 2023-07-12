using Students.Command.Test.Protos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task1.CQRS_MediatR_Using_gRPC.Data.Entities;
using Task1.CQRS_MediatR_Using_gRPC.Enums;
using Task1.CQRS_MediatR_Using_gRPC.Events;

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
}
