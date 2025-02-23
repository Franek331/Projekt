using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SeeSharp.Egzaminer.Domain.Entities;

namespace SeeSharp.Egzaminer.Infrastructure.Persistence.EntityConfigurations;

public class TestPublicationEntityMap : IEntityTypeConfiguration<TestPublication>
{
    public void Configure(EntityTypeBuilder<TestPublication> builder)
    {
        builder.ToTable("TestPublications");

        builder.Property(tp => tp.TestId)
            .IsRequired();

        builder.Property(tp => tp.StartDate)
            .IsRequired();

        builder.Property(tp => tp.Status)
            .IsRequired()
            .HasMaxLength(50);

        builder.HasOne(tp => tp.Test)
            .WithMany(tp => tp.TestPublications)
            .HasForeignKey(tp => tp.TestId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}
