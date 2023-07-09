using MediatR;
using Task1.CQRS_MediatR_Using_gRPC.Models;

namespace Task1.CQRS_MediatR_Using_gRPC.Commands;

public record AddStudentCommand(
    string Name,
    string Address,
    string Phone_Number) : IRequest<Student>;




