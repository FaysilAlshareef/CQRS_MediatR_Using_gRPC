using Task1.CQRS_MediatR_Using_gRPC.Events;

namespace Task1.CQRS_MediatR_Using_gRPC.Services;

public interface IServiceBusEventSender
{
    Task SendEventAsync(Event @event);

}
