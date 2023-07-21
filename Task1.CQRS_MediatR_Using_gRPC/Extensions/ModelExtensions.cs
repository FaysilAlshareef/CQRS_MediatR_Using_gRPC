namespace Task1.CQRS_MediatR_Using_gRPC.Extensions;
using Google.Protobuf.Collections;
using Google.Protobuf.WellKnownTypes;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Task1.CQRS_MediatR_Using_gRPC.Events;
using Task1.CQRS_MediatR_Using_gRPC.Protos;
using Student = Task1.CQRS_MediatR_Using_gRPC.Models.Student;

public static class ModelExtensions
{
    public static StudentOutput ToOutput(this Student student)
        => new()
        {
            Id = student.Id.ToString(),
            Name = student.Name,
            Address = student.Address,
            PhoneNumber = student.PhoneNumber
        };

    public static RepeatedField<EventMessage> ToOutputEvent(this RepeatedField<EventMessage> eventsOutput, IEnumerable<Event> events)
    {
        eventsOutput.AddRange(events.Select(e => new EventMessage
        {
            CorrelationId = e.Id.ToString(),
            DateTime = Timestamp.FromDateTime(DateTime.SpecifyKind(e.DateTime, DateTimeKind.Utc)),
            Data = JsonConvert.SerializeObject(((dynamic)e).Data, new StringEnumConverter()),
            Type = e.Type.ToString(),
            Sequence = e.Sequence,
            Version = e.Version,
            AggregateId = e.AggregateId.ToString(),
            UserId = e.UserId
        }));

        return eventsOutput;
    }
}
