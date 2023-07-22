using Task1.Command.Domain.Models;

namespace Task1.Command.Application.Contracts.Services.BaseService;
public interface ICommitEventService
{
    Task CommitNewEventsAsync(Student student);

}
