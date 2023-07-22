

using Task1.Command.Domain.Entities;

namespace Task1.Command.Application.Contracts.Repositories;

public interface IOutboxMassegesRepository : IAsyncRepository<OutboxMessage>
{

}
