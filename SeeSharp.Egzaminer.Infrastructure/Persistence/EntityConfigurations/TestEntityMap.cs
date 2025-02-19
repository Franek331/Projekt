using Microsoft.EntityFrameworkCore;
using SeeSharp.Egzaminer.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SeeSharp.Egzaminer.Infrastructure.Persistence.EntityConfigurations;

public class TestEntityMap : IEntityTypeConfiguration<Test>
{
    public void Configure(EntityTypeBuilder<Test> builder)
    {
        builder.ToTable("Tests");

        builder.Property(t => t.Title)
            .IsRequired()
            .HasMaxLength(256);

        builder.Property(t => t.Description)
            .HasMaxLength(5000);

        builder
            .HasMany(t => t.Questions)
            .WithMany(q => q.Tests)
            .UsingEntity<Dictionary<string, object>>(
                "TestQuestion",
                j => j
                    .HasOne<Question>()
                    .WithMany()
                    .HasForeignKey("QuestionId")
                    .HasConstraintName("FK_TestQuestion_QuestionId")
                    .OnDelete(DeleteBehavior.Cascade),
                j => j
                    .HasOne<Test>()
                    .WithMany()
                    .HasForeignKey("TestId")
                    .HasConstraintName("FK_TestQuestion_TestId")
                    .OnDelete(DeleteBehavior.Cascade));
    }
}
