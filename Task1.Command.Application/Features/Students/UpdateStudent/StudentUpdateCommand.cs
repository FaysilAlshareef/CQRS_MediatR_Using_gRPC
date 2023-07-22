using MediatR;
using Task1.Command.Domain.Commands;
using Task1.Command.Domain.Models;

namespace Task1.Command.Application.Features.UpdateStudent;


public record StudentUpdateCommand(
    Guid StudentId,
    string Name,
    string Phone_Number) : IRequest<Student>, IStudentUpdateCommand;



