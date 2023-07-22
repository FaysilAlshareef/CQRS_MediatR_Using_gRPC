using Google.Protobuf.WellKnownTypes;
using Grpc.Core;
using Task1.Command.Application.Contracts.Services.ServiceBus;
using Task1.Command.Domain.Events;
using Task1.Command.Domain.Events.DataTypes;
using Task1.Command.Domain.Exceptions;
using Task1.CQRS_MediatR_Using_gRPC.Protos;

namespace Task1.CQRS_MediatR_Using_gRPC.GrpcServices;

public class DemoEventService : DemoEvents.DemoEventsBase
{
    private readonly IServiceBusEventSender _serviceBusEventSender;

    public DemoEventService(IServiceBusEventSender serviceBusEventSender, IWebHostEnvironment env)
    {
        if (!env.IsDevelopment())
        {
            throw new AppException(ExceptionStatusCode.Cancelled, "env.IsDevelopment() == false in DemoEventsService");

        }
        _serviceBusEventSender = serviceBusEventSender;
    }

    public async override Task<Empty> Create(CreateDemoRequest request, ServerCallContext context)
    {
        var aggregateId = Guid.TryParse(request.Id, out var id) ? id : Guid.NewGuid();
        var studentCreated = new StudentAddedEvent(
            aggregateId,
            request.UserId,
            new StudentAddedData(
                request.Name,
                request.Address,
                request.PhoneNumber
                ));


        await _serviceBusEventSender.SendEventAsync(studentCreated);

        return new Empty();
    }


    public async override Task<Empty> Update(UpdateDemoRequest request, ServerCallContext context)
    {
        var aggregateId = Guid.TryParse(request.Id, out var id) ? id : Guid.NewGuid();
        var studentUpdated = new StudentUpdatedEvent(
            aggregateId,
            request.UserId,
            request.Sequence,
            new StudentUpdatedData(
                aggregateId,
                request.Name,
                request.PhoneNumber
                ));


        await _serviceBusEventSender.SendEventAsync(studentUpdated);

        return new Empty();
    }
}
