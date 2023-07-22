using System.Text.Json.Serialization;
using Task1.Command.Domain.Enums;

namespace Task1.Command.Domain.Events.DataTypes;

public interface IEventData
{
    [JsonIgnore]
    EventType Type { get; }
}
