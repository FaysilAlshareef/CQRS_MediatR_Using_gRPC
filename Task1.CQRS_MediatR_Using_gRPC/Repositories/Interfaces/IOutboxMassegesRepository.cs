using Task1.CQRS_MediatR_Using_gRPC.Data.Entities;

namespace Task1.CQRS_MediatR_Using_gRPC.Repositories.Interfaces;

public interface IOutboxMassegesRepository
{
    Task<IEnumerable<OutboxMessage>> GetAllAsync();
    void Remove(OutboxMessage message);

}
