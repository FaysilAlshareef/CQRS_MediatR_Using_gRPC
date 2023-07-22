using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Task1.Command.Domain.Enums;
using Task1.Command.Domain.Events;

namespace Task1.Command.Infra.Persistence.Configurations;

public class EventConfiguration : IEntityTypeConfiguration<Event>
{
    public void Configure(EntityTypeBuilder<Event> builder)
    {
        builder.Property(e => e.Type)
               .HasMaxLength(128)
               .HasConversion<string>();

        builder.HasDiscriminator(e => e.Type)
               .HasValue<StudentAddedEvent>(EventType.StudentAdded)
               .HasValue<StudentUpdatedEvent>(EventType.StudentUpdated);
    }
}
