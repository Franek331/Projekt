using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SeeSharp.Egzaminer.Domain.Entities;

namespace SeeSharp.Egzaminer.Infrastructure.Persistence.EntityConfigurations;

public class AccessTokenEntityMap : IEntityTypeConfiguration<AccessToken>
{
    public void Configure(EntityTypeBuilder<AccessToken> builder)
    {
        builder.Property(x => x.TokenValue).IsRequired().HasMaxLength(40);
        builder.Property(x => x.AttemptsUsed).IsRequired();
        builder.HasOne(x => x.TestPublication).WithMany().HasForeignKey(x => x.TestPublicationId).IsRequired();
        builder.HasOne(x => x.User).WithMany().HasForeignKey(x => x.UserId).IsRequired(false);
    }
}
