using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Task1.CQRS_MediatR_Using_gRPC.Events.DataTypes;
using Task1.CQRS_MediatR_Using_gRPC.Events;
using Newtonsoft.Json;

namespace Task1.CQRS_MediatR_Using_gRPC.Data.Configurations;

public class GenericEventConfiguration<TEntity, TData> : IEntityTypeConfiguration<TEntity>
        where TEntity : Event<TData>
        where TData : IEventData
{
    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.Property(e => e.Data).HasConversion(
            v => JsonConvert.SerializeObject(v),
            v => JsonConvert.DeserializeObject<TData>(v)
        ).HasColumnName("Data");
    }
}
