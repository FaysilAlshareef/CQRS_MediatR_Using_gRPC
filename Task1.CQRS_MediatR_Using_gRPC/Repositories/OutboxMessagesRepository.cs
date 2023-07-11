using Microsoft.EntityFrameworkCore;
using Task1.CQRS_MediatR_Using_gRPC.Data;
using Task1.CQRS_MediatR_Using_gRPC.Data.Entities;
using Task1.CQRS_MediatR_Using_gRPC.Repositories.Interfaces;

namespace Task1.CQRS_MediatR_Using_gRPC.Repositories;

public class OutboxMessagesRepository : IOutboxMassegesRepository
{
    private readonly ApplicationDbContext _context;

    public OutboxMessagesRepository(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<IEnumerable<OutboxMessage>> GetAllAsync()
     => await _context.OutboxMessages.Include(o => o.Event).ToListAsync();


    public void Remove(OutboxMessage message)
     => _context.OutboxMessages.Remove(message);
}
