using Task1.CQRS_MediatR_Using_gRPC.Data;
using Task1.CQRS_MediatR_Using_gRPC.Repositories.Interfaces;

namespace Task1.CQRS_MediatR_Using_gRPC.Repositories;

public class UnitOfWork : IUnitOfWork
{
    private readonly ApplicationDbContext _context;

    public UnitOfWork(ApplicationDbContext context)
    {
        _context = context;
        OutboxMessages = new OutboxMessagesRepository(context);
        Events = new EventsRepository(context);
    }
    public IOutboxMassegesRepository OutboxMessages { get; }

    public IEventsRepository Events { get; }

    public async Task<int> CompleteAsync()
    => await _context.SaveChangesAsync();


    public void Dispose()
    {
        _context.Dispose();
    }
}
