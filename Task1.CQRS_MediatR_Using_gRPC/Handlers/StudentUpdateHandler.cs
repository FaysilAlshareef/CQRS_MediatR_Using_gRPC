using Grpc.Core;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Task1.CQRS_MediatR_Using_gRPC.Commands;
using Task1.CQRS_MediatR_Using_gRPC.Data;
using Task1.CQRS_MediatR_Using_gRPC.Models;
using Task1.CQRS_MediatR_Using_gRPC.ReturnMessages;

namespace Task1.CQRS_MediatR_Using_gRPC.Handlers;

public class StudentUpdateHandler : IRequestHandler<StudentUpdateCommand, Student>
{
    private readonly ApplicationDbContext _context;

    public StudentUpdateHandler(ApplicationDbContext context)
    {
        _context = context;
    }
    public async Task<Student> Handle(StudentUpdateCommand command, CancellationToken cancellationToken)
    {
        if (await _context.UniqueReferences.AnyAsync(u => u.Name == command.Name && u.Id != command.studentId, cancellationToken))
        {
            throw new RpcException(new Status(StatusCode.AlreadyExists, ""));
        }
        var events = await _context.EventStore.Where(e => e.AggregateId == command.studentId)
                                             .OrderBy(e => e.Sequence)
                                             .ToListAsync(cancellationToken);

        if (events.Count == 0)
            throw new RpcException(new Status(StatusCode.NotFound, "MESSAGE HERE"));

        var student = Student.LoadFromHistory(events);
        if (student?.Name != command.Name)
            await _context.UniqueReferences.Where(u => u.Id == command.studentId)
                                           .ExecuteUpdateAsync(x => x.SetProperty(s => s.Name, command.Name));
        student.Update(command);

        await _context.CommitNewEventsAsync(student);
        return student;
    }
}
