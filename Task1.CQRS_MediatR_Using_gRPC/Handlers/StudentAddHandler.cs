using Grpc.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Task1.CQRS_MediatR_Using_gRPC.Commands;
using Task1.CQRS_MediatR_Using_gRPC.Data;
using Task1.CQRS_MediatR_Using_gRPC.Data.Entities;
using Task1.CQRS_MediatR_Using_gRPC.Exceptions;
using Task1.CQRS_MediatR_Using_gRPC.Models;
using Task1.CQRS_MediatR_Using_gRPC.Resources;
using Task1.CQRS_MediatR_Using_gRPC.ReturnMessages;

namespace Task1.CQRS_MediatR_Using_gRPC.Handlers;

public class StudentAddHandler : IRequestHandler<StudentAddCommand, Student>
{
    private readonly ApplicationDbContext _context;

    public StudentAddHandler(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Student> Handle(StudentAddCommand command, CancellationToken cancellationToken)
    {
        if (await _context.UniqueReferences.AnyAsync(u => u.Name == command.Name, cancellationToken))
        {
            throw new AppException(ExceptionStatusCode.AlreadyExists, Phrases.StudentNameAlreadyExist);
        }

        var student = Student.Create(command);
        await _context.UniqueReferences.AddAsync(new UniqueReference(student));

        await _context.CommitNewEventsAsync(student);

        return student;
    }
}
