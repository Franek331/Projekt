using Microsoft.EntityFrameworkCore;
using SeeSharp.Egzaminer.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace SeeSharp.Egzaminer.Infrastructure.Persistence.EntityConfigurations;

public class QuestionAnswerEntityMap : IEntityTypeConfiguration<QuestionAnswer>
{
    public void Configure(EntityTypeBuilder<QuestionAnswer> builder)
    {
        builder.Property(x => x.Content)
            .IsRequired();
        builder.Property(x => x.IsCorrect)
            .IsRequired();

        builder.HasOne(x => x.Question)
            .WithMany(x => x.Answers)
            .HasForeignKey(x => x.QuestionId)
            .IsRequired();
    }
}
