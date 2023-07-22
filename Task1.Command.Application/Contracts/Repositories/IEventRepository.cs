

using Task1.Command.Domain.Events;

namespace Task1.Command.Application.Contracts.Repositories;

public interface IEventRepository : IAsyncRepository<Event>
{
    Task<IEnumerable<Event>> GetAllByAggregateIdAsync(Guid aggregateId, CancellationToken cancellationToken);
    Task<IEnumerable<Event>> GetAsPaginationAsync(int currentPage = 1, int pageSize = 100);

}
