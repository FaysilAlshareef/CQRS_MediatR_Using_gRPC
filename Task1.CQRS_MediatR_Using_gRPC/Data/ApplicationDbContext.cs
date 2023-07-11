using MediatR;
using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Task1.CQRS_MediatR_Using_gRPC.Data.Configurations;
using Task1.CQRS_MediatR_Using_gRPC.Data.Entities;
using Task1.CQRS_MediatR_Using_gRPC.Enums;
using Task1.CQRS_MediatR_Using_gRPC.Events;
using Task1.CQRS_MediatR_Using_gRPC.Events.DataTypes;
using Task1.CQRS_MediatR_Using_gRPC.Models;
using Task1.CQRS_MediatR_Using_gRPC.Services;

namespace Task1.CQRS_MediatR_Using_gRPC.Data;

public class ApplicationDbContext : DbContext
{
    private readonly ServiceBusPublisher _publisher;

    public ApplicationDbContext(DbContextOptions options, ServiceBusPublisher publisher) : base(options)
    {
        _publisher = publisher;
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfiguration(new EventConfiguration());
        modelBuilder.ApplyConfiguration(new GenericEventConfiguration<StudentAddedEvent, StudentAddedData>());
        modelBuilder.ApplyConfiguration(new GenericEventConfiguration<StudentUpdatedEvent, StudentUpdatedData>());
        modelBuilder.ApplyConfiguration(new UniqueReferenceConfiguration());
        modelBuilder.ApplyConfiguration(new OutboxMessageConfiguration());


        base.OnModelCreating(modelBuilder);
    }
    public DbSet<UniqueReference> UniqueReferences { get; set; }
    public DbSet<Event> EventStore { get; set; }
    public DbSet<OutboxMessage> OutboxMessages { get; set; }


    public async Task CommitNewEventsAsync(Student student)
    {
        var newEvents = student.GetUncommittedEvents();

        await EventStore.AddRangeAsync(newEvents);

        var messages = OutboxMessage.ToManyMessages(newEvents);

        await OutboxMessages.AddRangeAsync(messages);

        await SaveChangesAsync();


        student.MarkChangesAsCommitted();
        _publisher.PublishAsync();

    }
}
