using MediatR;
using Task1.CQRS_MediatR_Using_gRPC.Commands;
using Task1.CQRS_MediatR_Using_gRPC.ReturnMessages;

namespace Task1.CQRS_MediatR_Using_gRPC.Handlers;

public class StudentUpdateHandler : IRequestHandler<StudentUpdateCommand, string>
{

    public async Task<string> Handle(StudentUpdateCommand command, CancellationToken cancellationToken)
    {

        return Messages.StudentUpdateSuccess;
    }
}
