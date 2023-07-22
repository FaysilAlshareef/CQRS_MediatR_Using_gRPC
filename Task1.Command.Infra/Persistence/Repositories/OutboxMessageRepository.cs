﻿using Microsoft.EntityFrameworkCore;
using Task1.Command.Application.Contracts.Repositories;
using Task1.Command.Domain.Entities;

namespace Task1.Command.Infra.Persistence.Repositories
{
    public class OutboxMessageRepository : AsyncRepository<OutboxMessage>, IOutboxMassegesRepository
    {
        private readonly ApplicationDbContext _appDbContext;

        public OutboxMessageRepository(ApplicationDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async override Task<IEnumerable<OutboxMessage>> GetAllAsync()
        {
            return await _appDbContext.OutboxMessages
                                      .Include(o => o.Event)
                                      .ToListAsync();
        }
    }
}
