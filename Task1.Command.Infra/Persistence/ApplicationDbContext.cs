using Microsoft.EntityFrameworkCore;
using Task1.Command.Domain.Entities;
using Task1.Command.Domain.Events;
using Task1.Command.Domain.Events.DataTypes;
using Task1.Command.Infra.Persistence.Configurations;

namespace Task1.Command.Infra.Persistence;

public class ApplicationDbContext : DbContext
{


    public ApplicationDbContext(DbContextOptions options) : base(options)
    {

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


    //public async Task CommitNewEventsAsync(Student student)
    //{
    //    var newEvents = student.GetUncommittedEvents();

    //    await EventStore.AddRangeAsync(newEvents);

    //    var messages = OutboxMessage.ToManyMessages(newEvents);

    //    await OutboxMessages.AddRangeAsync(messages);

    //    await SaveChangesAsync();


    //    student.MarkChangesAsCommitted();
    //    _publisher.PublishAsync();

    //}
}
