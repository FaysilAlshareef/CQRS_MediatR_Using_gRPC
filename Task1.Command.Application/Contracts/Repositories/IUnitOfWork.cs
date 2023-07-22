
namespace Task1.Command.Application.Contracts.Repositories;

public interface IUnitOfWork : IDisposable
{
    IUniqueUniqueReferenceRepository UniqueReference { get; }
    IOutboxMassegesRepository OutboxMessages { get; }
    IEventRepository Events { get; }
    Task<int> CompleteAsync();
}
