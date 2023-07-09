﻿using Grpc.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Task1.CQRS_MediatR_Using_gRPC.Commands;
using Task1.CQRS_MediatR_Using_gRPC.Data;
using Task1.CQRS_MediatR_Using_gRPC.Data.Entities;
using Task1.CQRS_MediatR_Using_gRPC.Models;
using Task1.CQRS_MediatR_Using_gRPC.ReturnMessages;

namespace Task1.CQRS_MediatR_Using_gRPC.Handlers;

public class AddStudentHandler : IRequestHandler<AddStudentCommand, Student>
{
    private readonly ApplicationDbContext _context;

    public AddStudentHandler(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Student> Handle(AddStudentCommand request, CancellationToken cancellationToken)
    {
        if (await _context.UniqueReferences.AnyAsync(u => u.Name == request.Name, cancellationToken))
        {
            throw new RpcException(new Status(StatusCode.AlreadyExists, ""));
        }

        var student = Student.Create(request);

        var newEvents = student.GetUncommittedEvents();
        await _context.EventStore.AddRangeAsync(newEvents);
        await _context.UniqueReferences.AddAsync(new UniqueReference(student.Id, student.Name));
        await _context.SaveChangesAsync();

        return student;
    }
}
