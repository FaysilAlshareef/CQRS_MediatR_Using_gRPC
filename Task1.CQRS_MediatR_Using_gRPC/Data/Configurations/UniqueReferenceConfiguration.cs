using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Task1.CQRS_MediatR_Using_gRPC.Data.Entities;

namespace Task1.CQRS_MediatR_Using_gRPC.Data.Configurations;

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
