using Microsoft.EntityFrameworkCore;
using SeeSharp.Egzaminer.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SeeSharp.Egzaminer.Infrastructure.Persistence.EntityConfigurations;

public class SystemLogEntityMap : IEntityTypeConfiguration<SystemLog>
{
    public void Configure(EntityTypeBuilder<SystemLog> builder)
    {
        builder.Property(x => x.Message).IsRequired();
        builder.Property(x => x.LogLevel).IsRequired();
        builder.Property(x => x.LogDate).IsRequired();
    }
}
