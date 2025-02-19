using Microsoft.EntityFrameworkCore;
using SeeSharp.Egzaminer.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SeeSharp.Egzaminer.Infrastructure.Persistence.EntityConfigurations;

public class QuestionEntityMap : IEntityTypeConfiguration<Question>
{
    public void Configure(EntityTypeBuilder<Question> builder)
    {
        builder.ToTable("Questions");
        builder.HasKey(x => x.Id);
        builder.Property(x => x.Content)
            .IsRequired()
            .HasMaxLength(9096);
        builder.Property(x => x.Points)
            .IsRequired();
        builder.Property(x => x.QuestionType)
            .IsRequired();
    }
}
