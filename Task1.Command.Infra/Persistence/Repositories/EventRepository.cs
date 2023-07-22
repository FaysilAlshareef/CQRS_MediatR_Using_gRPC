using Microsoft.EntityFrameworkCore;
using Task1.Command.Application.Contracts.Repositories;
using Task1.Command.Domain.Events;

namespace Task1.Command.Infra.Persistence.Repositories
{
    public class EventRepository : AsyncRepository<Event>, IEventRepository
    {
        private readonly ApplicationDbContext _appDbContext;

        public EventRepository(ApplicationDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<IEnumerable<Event>> GetAllByAggregateIdAsync(Guid aggregateId, CancellationToken cancellationToken)
            => await _appDbContext.EventStore
                                  .AsNoTracking()
                                  .Where(e => e.AggregateId == aggregateId)
                                  .OrderBy(e => e.Sequence)
                                  .ToListAsync(cancellationToken);

        public async Task<IEnumerable<Event>> GetAsPaginationAsync(int currentPage = 1, int pageSize = 2)
        {
            var skip = (currentPage - 1) * pageSize;

            return await _appDbContext.EventStore
                                      .AsNoTracking()
                                      .OrderBy(e => e.AggregateId)
                                      .ThenBy(e => e.Sequence)
                                      .Skip(skip)
                                      .Take(pageSize)
                                      .ToListAsync();
        }
    }
}
