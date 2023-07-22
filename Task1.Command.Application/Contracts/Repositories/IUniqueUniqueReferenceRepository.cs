using Task1.Command.Domain.Entities;

namespace Task1.Command.Application.Contracts.Repositories
{
    public interface IUniqueUniqueReferenceRepository : IAsyncRepository<UniqueReference>
    {
        Task<long> GetMaxAsync();
        Task<bool> IsExistAsync(string studentName, CancellationToken cancellationToken);
        Task<bool> IsExistAsync(string studentName, Guid studentId, CancellationToken cancellationToken);
    }
}
