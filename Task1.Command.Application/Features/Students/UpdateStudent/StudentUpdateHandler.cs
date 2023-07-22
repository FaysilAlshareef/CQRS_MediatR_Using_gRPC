using MediatR;
using Task1.Command.Application.Contracts.Repositories;
using Task1.Command.Application.Contracts.Services.BaseService;
using Task1.Command.Application.Features.UpdateStudent;
using Task1.Command.Application.Resources;
using Task1.Command.Domain.Exceptions;
using Task1.Command.Domain.Models;

namespace Task1.Command.Application.Features.AddStudent;


public class StudentUpdateHandler : IRequestHandler<StudentUpdateCommand, Student>
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly ICommitEventService _commitEventService;

    public StudentUpdateHandler(IUnitOfWork unitOfWork, ICommitEventService commitEventService)
    {

        _unitOfWork = unitOfWork;
        _commitEventService = commitEventService;
    }
    public async Task<Student> Handle(StudentUpdateCommand command, CancellationToken cancellationToken)
    {
        if (await _unitOfWork.UniqueReference.IsExistAsync(command.Name, command.StudentId, cancellationToken))
        {
            throw new AppException(ExceptionStatusCode.AlreadyExists, Phrases.StudentNameAlreadyExist);
        }
        var events = await _unitOfWork.Events.GetAllByAggregateIdAsync(command.StudentId, cancellationToken);
        if (!events.Any())
            throw new AppException(ExceptionStatusCode.NotFound, Phrases.StudentNotFound);


        var student = Student.LoadFromHistory(events);
        if (student?.Name != command.Name)
        {
            var uniqueReference = await _unitOfWork.UniqueReference.FindAsync(command.StudentId);

            uniqueReference.Update(command.Name);
        }

        student.Update(command);

        await _commitEventService.CommitNewEventsAsync(student);

        return student;
    }
}
