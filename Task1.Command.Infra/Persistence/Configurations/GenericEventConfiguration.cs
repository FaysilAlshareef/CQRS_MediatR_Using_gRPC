using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Newtonsoft.Json;
using Task1.Command.Domain.Events;
using Task1.Command.Domain.Events.DataTypes;

namespace Task1.Command.Infra.Persistence.Configurations;

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
