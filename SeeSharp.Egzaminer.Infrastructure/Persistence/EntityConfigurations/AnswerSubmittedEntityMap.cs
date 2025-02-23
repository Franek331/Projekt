// AnswerSubmittedEntityMap.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SeeSharp.Egzaminer.Domain.Entities;

namespace SeeSharp.Egzaminer.Infrastructure.Persistence.EntityConfigurations
{
    public class AnswerSubmittedEntityMap : IEntityTypeConfiguration<AnswerSubmitted>
    {
        public void Configure(EntityTypeBuilder<AnswerSubmitted> builder)
        {
            builder.Property(x => x.ProvidedAnswer)
                .IsRequired(false);
            builder.Property(x => x.SelectedOptions)
                .IsRequired(false);
            builder.Property(x => x.AutoScore)
                .IsRequired();
            builder.Property(x => x.ManualScore)
                .IsRequired();
            builder.Property(x => x.ManualComment)
                .IsRequired(false);

            builder.HasOne(x => x.TestSubmission)
                .WithMany(s => s.Answers)
                .HasForeignKey(x => x.TestSubmissionId)
                .IsRequired();

            // Ustalamy relację z Question jako wymaganą (zgodnie z definicją encji)
            builder.HasOne(x => x.Question)
                .WithMany() // Jeśli encja Question nie posiada kolekcji AnswerSubmitted
                .HasForeignKey(x => x.QuestionId)
                .IsRequired();

            builder.HasOne(x => x.TestPublication)
                .WithMany(tp => tp.AnswerSubmitted)  // Zakładając, że TestPublication ma kolekcję AnswerSubmitted
                .HasForeignKey(x => x.TestPublicationId)
                .OnDelete(DeleteBehavior.NoAction);

            // Konfiguracja relacji z ManualGradingResults – określamy kolekcję po stronie AnswerSubmitted
            builder.HasMany(x => x.ManualGradingResults)
                .WithOne(mg => mg.AnswerSubmitted)
                .HasForeignKey(mg => mg.AnswerSubmittedId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
