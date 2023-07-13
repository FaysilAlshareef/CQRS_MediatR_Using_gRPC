using Microsoft.EntityFrameworkCore;
using System;
using Task1.CQRS_MediatR_Using_gRPC.Commands;
using Task1.CQRS_MediatR_Using_gRPC.Data;
using Task1.CQRS_MediatR_Using_gRPC.Data.Entities;
using Task1.CQRS_MediatR_Using_gRPC.Events;
using Task1.CQRS_MediatR_Using_gRPC.Models;

namespace Task1.CQRS_MediatR_Using_gRPC.Extensions;

public static class ApplicationDbContextExtension
{
    public static async Task BuildUniqueRecordsAsync(
            this ApplicationDbContext context,
            ILogger logger = null
        )
    {
        if (await context.UniqueReferences.AnyAsync())
            return;

        var events = await context.EventStore
                                  .OfType<StudentAddedEvent>()
                                  .ToListAsync();

        foreach (var @event in events)
        {
            var data = @event.Data;

            var createCommand = new StudentAddCommand(
                data.Name,
                data.Address,
                data.Phone_Number);

            var student = Student.Create(createCommand);

            var uniqueReference = new UniqueReference(student);

            await context.UniqueReferences.AddAsync(uniqueReference);
        }

        try
        {
            await context.SaveChangesAsync();
        }
        catch (Exception e)
        {
            logger?.LogError(e, "Build unique tables failed.");
            throw;
        }
    }

}
