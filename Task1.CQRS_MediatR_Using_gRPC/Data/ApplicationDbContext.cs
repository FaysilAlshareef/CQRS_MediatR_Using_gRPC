using Microsoft.EntityFrameworkCore;
using System.Reflection;
using Task1.CQRS_MediatR_Using_gRPC.Data.Configurations;
using Task1.CQRS_MediatR_Using_gRPC.Data.Entities;
using Task1.CQRS_MediatR_Using_gRPC.Events;
using Task1.CQRS_MediatR_Using_gRPC.Events.DataTypes;

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
}
