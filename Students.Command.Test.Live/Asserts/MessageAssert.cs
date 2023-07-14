using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Task1.CQRS_MediatR_Using_gRPC.Commands;
using Task1.CQRS_MediatR_Using_gRPC.Events;
using Task1.CQRS_MediatR_Using_gRPC.Events.DataTypes;
using Task1.CQRS_MediatR_Using_gRPC.Services;

namespace Students.Command.Test.Live.Asserts;
public class MessageAssert
{
    public static void AssertEquality(
        StudentAddedEvent createdEvent,
        ServiceBusReceivedMessage message
        )
    {
        var body = JsonConvert.DeserializeObject<MessageBody>(Encoding.UTF8.GetString(message.Body));

        BaseAssert(createdEvent, message, body);

        var eventData = createdEvent.Data;

        var messageData = JsonConvert.DeserializeObject<StudentAddedData>(body.Data.ToString());

        Assert.Equal(eventData.Name, messageData.Name);
        Assert.Equal(eventData.Address, messageData.Address);
        Assert.Equal(eventData.Phone_Number, messageData.Phone_Number);
    }

    public static void AssertEquality(
    StudentUpdatedEvent createdEvent,
    ServiceBusReceivedMessage message
    )
    {
        var body = JsonConvert.DeserializeObject<MessageBody>(Encoding.UTF8.GetString(message.Body));

        BaseAssert(createdEvent, message, body);

        var eventData = createdEvent.Data;

        var messageData = JsonConvert.DeserializeObject<StudentUpdatedData>(body.Data.ToString());

        Assert.Equal(eventData.Name, messageData.Name);
        Assert.Equal(eventData.StudentId, messageData.StudentId);
        Assert.Equal(eventData.Phone_Number, messageData.Phone_Number);
    }


    private static void BaseAssert(Event studentEvent, ServiceBusReceivedMessage message, MessageBody body)
    {
        Assert.NotNull(studentEvent);
        Assert.NotNull(message);
        Assert.Equal(studentEvent.Id.ToString(), message.CorrelationId);

        Assert.NotNull(body);
        Assert.NotNull(body.Data);

        Assert.Equal(studentEvent.Sequence, body.Sequence);
        Assert.Equal(studentEvent.Version, body.Version);
        Assert.Equal(studentEvent.Type.ToString(), body.Type);
        Assert.Equal(studentEvent.DateTime, body.DateTime, TimeSpan.FromMinutes(1));
    }
}
