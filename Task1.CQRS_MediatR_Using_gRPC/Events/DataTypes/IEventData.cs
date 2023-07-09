using System.Text.Json.Serialization;
using Task1.CQRS_MediatR_Using_gRPC.Enums;

namespace Task1.CQRS_MediatR_Using_gRPC.Events.DataTypes;

public interface IEventData
{
    [JsonIgnore]
    EventType Type { get; }
}
