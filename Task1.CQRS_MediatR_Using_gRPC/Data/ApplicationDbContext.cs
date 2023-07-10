using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Task1.CQRS_MediatR_Using_gRPC.Data.Configurations;
using Task1.CQRS_MediatR_Using_gRPC.Data.Entities;
using Task1.CQRS_MediatR_Using_gRPC.Enums;
using Task1.CQRS_MediatR_Using_gRPC.Events;
using Task1.CQRS_MediatR_Using_gRPC.Events.DataTypes;
using Task1.CQRS_MediatR_Using_gRPC.Models;

namespace Task1.CQRS_MediatR_Using_gRPC.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new EventConfiguration());
        modelBuilder.ApplyConfiguration(new GenericEventConfiguration<StudentAddedEvent, StudentAddedData>());
        modelBuilder.ApplyConfiguration(new UniqueReferenceConfiguration());


        base.OnModelCreating(modelBuilder);
    }
    public DbSet<UniqueReference> UniqueReferences { get; set; }
    public DbSet<Event> EventStore { get; set; }


    public async Task CommitNewEventsAsync(Student student)
    {
        var newEvents = student.GetUncommittedEvents();

        await EventStore.AddRangeAsync(newEvents);

        //var messages = OutboxMessage.ToManyMessages(newEvents);

        //await OutboxMessages.AddRangeAsync(messages);

        foreach (var @event in newEvents)
        {
            if (@event.Type == EventType.StudentAdded)
                await UniqueReferences.AddAsync(new UniqueReference(student));
        }

        await SaveChangesAsync();


        //student.MarkChangesAsCommitted();

        //await _publisher.PublishAsync(messages);
    }
}
