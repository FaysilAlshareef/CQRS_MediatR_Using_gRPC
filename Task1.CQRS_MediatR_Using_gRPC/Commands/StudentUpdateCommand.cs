using MediatR;

namespace Task1.CQRS_MediatR_Using_gRPC.Commands;

public record StudentUpdateCommand(
    string Name,
    string Phone_Number) : IRequest<string>;



