using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Task1.CQRS_MediatR_Using_gRPC.Enums;
using Task1.CQRS_MediatR_Using_gRPC.Events;

namespace Task1.CQRS_MediatR_Using_gRPC.Data.Configurations;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.Property(e => e.Type)
               .HasMaxLength(128)
               .HasConversion<string>();

        builder.HasDiscriminator(e => e.Type)
               .HasValue<StudentAddedEvent>(EventType.StudentAdded);
    }
}
