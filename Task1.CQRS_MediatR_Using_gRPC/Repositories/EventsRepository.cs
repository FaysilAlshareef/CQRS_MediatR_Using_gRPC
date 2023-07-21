using Microsoft.EntityFrameworkCore;
using Task1.CQRS_MediatR_Using_gRPC.Data;
using Task1.CQRS_MediatR_Using_gRPC.Events;
using Task1.CQRS_MediatR_Using_gRPC.Repositories.Interfaces;

namespace Task1.CQRS_MediatR_Using_gRPC.Repositories;

public class EventsRepository : IEventsRepository
{
    private readonly ApplicationDbContext _dbContext;

    public EventsRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    public async Task<IEnumerable<Event>> GetAsPaginationAsync(int currentPage, int pageSize)
    {
        var skip = (currentPage - 1) * pageSize;

        return await _dbContext.EventStore
                                  .AsNoTracking()
                                  .OrderBy(e => e.AggregateId)
                                  .ThenBy(e => e.Sequence)
                                  .Skip(skip)
                                  .Take(pageSize)
                                  .ToListAsync();
    }
}
