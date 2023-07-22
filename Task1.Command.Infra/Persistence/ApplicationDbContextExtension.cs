using Microsoft.EntityFrameworkCore;
using Serilog;
using Task1.Command.Domain.Entities;
using Task1.Command.Domain.Enums;
using Task1.Command.Domain.Models;

namespace Task1.Command.Infra.Persistence;


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
                                  .Where(e => e.Type == EventType.StudentAdded)
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
            logger?.Error(e, "Build unique tables failed.");
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
            logger?.Error(e, "Build unique tables failed.");
            throw;
        }
    }

}
