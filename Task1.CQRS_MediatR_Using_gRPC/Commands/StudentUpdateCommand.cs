using MediatR;
using Task1.CQRS_MediatR_Using_gRPC.Models;

namespace Task1.CQRS_MediatR_Using_gRPC.Commands;

public record StudentUpdateCommand(
    Guid studentId,
    string Name,
    string Phone_Number) : IRequest<Student>;



