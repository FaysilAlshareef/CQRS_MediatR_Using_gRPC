using Task1.Command.Application.Contracts.Repositories;

namespace Task1.Command.Infra.Persistence.Repositories
{
    internal class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _appDbContext;

        public UnitOfWork(ApplicationDbContext appDbContext)
        {
            _appDbContext = appDbContext;
        }

        private IEventRepository _events;
        public IEventRepository Events
        {
            get
            {
                if (_events != null)
                    return _events;

                return _events = new EventRepository(_appDbContext);
            }
        }

        private IOutboxMassegesRepository _outboxMessages;
        public IOutboxMassegesRepository OutboxMessages
        {
            get
            {
                if (_outboxMessages != null)
                    return _outboxMessages;

                return _outboxMessages = new OutboxMessageRepository(_appDbContext);
            }
        }


        public IUniqueUniqueReferenceRepository UniqueReference
        {
            get
            {
                if (_uniqueReference != null)
                    return _uniqueReference;

                return _uniqueReference = new UniqueReferenceRepository(_appDbContext);
            }
        }

        public IUniqueUniqueReferenceRepository _uniqueReference;

        public async Task<int> CompleteAsync()
          => await _appDbContext.SaveChangesAsync();


        public void Dispose()
        {
            _appDbContext.Dispose();
        }


    }
}
