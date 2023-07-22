using MediatR;
using Task1.Command.Application.Contracts.Repositories;
using Task1.Command.Application.Contracts.Services.BaseService;
using Task1.Command.Application.Resources;
using Task1.Command.Domain.Entities;
using Task1.Command.Domain.Exceptions;
using Task1.Command.Domain.Models;

namespace Task1.Command.Application.Features.AddStudent;


public class StudentAddHandler : IRequestHandler<StudentAddCommand, Student>
{

    private readonly IUnitOfWork _unitOfWork;
    private readonly ICommitEventService _commitEventService;

    public StudentAddHandler(IUnitOfWork unitOfWork, ICommitEventService commitEventService)
    {

        _unitOfWork = unitOfWork;
        _commitEventService = commitEventService;
    }
    public async Task<Student> Handle(StudentAddCommand command, CancellationToken cancellationToken)
    {
        if (await _unitOfWork.UniqueReference.IsExistAsync(command.Name, cancellationToken))
        {
            throw new AppException(ExceptionStatusCode.AlreadyExists, Phrases.StudentNameAlreadyExist);
        }

        var student = Student.Create(command);
        await _unitOfWork.UniqueReference.AddAsync(new UniqueReference(student));

        await _commitEventService.CommitNewEventsAsync(student);


        return student;
    }
}
