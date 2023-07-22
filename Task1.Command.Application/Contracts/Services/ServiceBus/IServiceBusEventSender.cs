

using Task1.Command.Domain.Events;

namespace Task1.Command.Application.Contracts.Services.ServiceBus;

public interface IServiceBusEventSender
{
    Task SendEventAsync(Event @event);

}
