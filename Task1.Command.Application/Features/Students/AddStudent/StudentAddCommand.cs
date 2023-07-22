using MediatR;
using Task1.Command.Domain.Commands;
using Task1.Command.Domain.Models;

namespace Task1.Command.Application.Features.AddStudent;

public record StudentAddCommand(
    string Name,
    string Address,
    string Phone_Number) : IRequest<Student>, IStudentAddCommand;




