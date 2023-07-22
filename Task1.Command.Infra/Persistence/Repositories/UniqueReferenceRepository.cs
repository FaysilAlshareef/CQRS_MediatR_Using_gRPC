using Microsoft.EntityFrameworkCore;
using Task1.Command.Application.Contracts.Repositories;
using Task1.Command.Domain.Entities;

namespace Task1.Command.Infra.Persistence.Repositories
{
    public class UniqueReferenceRepository : AsyncRepository<UniqueReference>, IUniqueUniqueReferenceRepository
    {
        private readonly ApplicationDbContext _appDbContext;

        public UniqueReferenceRepository(ApplicationDbContext appDbContext) : base(appDbContext)
        {
            _appDbContext = appDbContext;
        }

        public async Task<bool> IsExistAsync(string studentName, CancellationToken cancellationToken)
            => await _appDbContext.UniqueReferences.AnyAsync(u => u.Name == studentName, cancellationToken);

        public async Task<bool> IsExistAsync(string studentName, Guid studentId, CancellationToken cancellationToken)
      => await _appDbContext.UniqueReferences.AnyAsync(u => u.Name == studentName && u.Id != studentId, cancellationToken);


        public async Task<long> GetMaxAsync()
            => await _appDbContext.UniqueReferences.CountAsync() + 1;


    }
}
