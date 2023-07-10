using Task1.CQRS_MediatR_Using_gRPC.Commands;
using Task1.CQRS_MediatR_Using_gRPC.Protos;

namespace Task1.CQRS_MediatR_Using_gRPC.Extensions;

public static class CommandExtensions
{

    public static StudentAddCommand ToCommand(this CreateRequest request)
        => new(request.Name, request.Address, request.PhoneNumber);

    public static StudentUpdateCommand ToCommand(this UpdateStudentRequest request)
        => new(request.Name, request.PhoneNumber);
}
