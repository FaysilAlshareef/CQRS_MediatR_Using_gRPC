using Task1.CQRS_MediatR_Using_gRPC.Events;

namespace Task1.CQRS_MediatR_Using_gRPC.Repositories.Interfaces;

public interface IEventsRepository
{
    Task<IEnumerable<Event>> GetAsPaginationAsync(int currentPage, int pageSize);
}
