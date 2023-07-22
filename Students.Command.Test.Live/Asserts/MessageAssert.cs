using Azure.Messaging.ServiceBus;
using Newtonsoft.Json;
using System.Text;
using Task1.Command.Domain.Events;
using Task1.Command.Domain.Events.DataTypes;
using Task1.Command.Infra.Services.ServiceBus;

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
