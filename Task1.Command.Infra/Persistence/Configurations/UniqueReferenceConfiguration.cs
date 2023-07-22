using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Task1.Command.Domain.Entities;

namespace Task1.Command.Infra.Persistence.Configurations;

public class UniqueReferenceConfiguration : IEntityTypeConfiguration<UniqueReference>
{
    public void Configure(EntityTypeBuilder<UniqueReference> builder)
    {
        builder.Property(e => e.Name)
                .IsRequired()
                .HasMaxLength(128);

        builder.HasIndex(e => e.Name).IsUnique();
    }
}
