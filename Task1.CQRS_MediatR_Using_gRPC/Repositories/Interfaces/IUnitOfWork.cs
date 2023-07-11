namespace Task1.CQRS_MediatR_Using_gRPC.Repositories.Interfaces;

public interface IUnitOfWork : IDisposable
{
    IOutboxMassegesRepository OutboxMessages { get; }
    Task<int> CompleteAsync();
}
