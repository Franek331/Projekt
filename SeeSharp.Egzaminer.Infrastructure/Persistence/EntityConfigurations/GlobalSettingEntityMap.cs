using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SeeSharp.Egzaminer.Domain.Entities;

namespace SeeSharp.Egzaminer.Infrastructure.Persistence.EntityConfigurations;

public class GlobalSettingEntityMap : IEntityTypeConfiguration<GlobalSetting>
{
    public void Configure(EntityTypeBuilder<GlobalSetting> builder)
    {
        builder.ToTable("GlobalSettings");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Key)
            .IsRequired()
            .HasMaxLength(255);
        builder.Property(x => x.Value)
            .IsRequired()
            .HasMaxLength(2048);
    }
}
