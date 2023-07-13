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
                                  .Where(e => e.Type == Enums.EventType.StudentAdded)
                                  .ToListAsync();

        var student = Student.LoadFromHistory(events);

        var uniqueReference = new UniqueReference(student);

        await context.UniqueReferences.AddAsync(uniqueReference);

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

    public static async Task BuildUniqueRecordsAsync(
          this ApplicationDbContext context,
          Guid aggregateId,
          ILogger logger = null
      )
    {
        if (await context.UniqueReferences.AnyAsync())
            return;

        var events = await context.EventStore
                                  .Where(e => e.AggregateId == aggregateId)
                                  .ToListAsync();

        var student = Student.LoadFromHistory(events);

        var uniqueReference = new UniqueReference(student);

        await context.UniqueReferences.AddAsync(uniqueReference);

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
