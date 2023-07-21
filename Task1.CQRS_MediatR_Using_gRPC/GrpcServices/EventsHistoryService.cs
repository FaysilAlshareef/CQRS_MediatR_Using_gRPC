using Grpc.Core;
using Task1.CQRS_MediatR_Using_gRPC.Extensions;
using Task1.CQRS_MediatR_Using_gRPC.Protos;
using Task1.CQRS_MediatR_Using_gRPC.Repositories.Interfaces;

namespace Task1.CQRS_MediatR_Using_gRPC.GrpcServices;

public class EventsHistoryService : EventsHistory.EventsHistoryBase
{
    private readonly IUnitOfWork _unitOfWork;

    public EventsHistoryService(IUnitOfWork unitOfWork)
    {
        _unitOfWork = unitOfWork;
    }

    public override async Task<Response> GetEvents(GetEventsRequest request, ServerCallContext context)
    {
        var events = await _unitOfWork.Events.GetAsPaginationAsync(request.CurrentPage, request.PageSize);

        var response = new Response();

        response.Events.ToOutputEvent(events);

        return response;
    }
}
