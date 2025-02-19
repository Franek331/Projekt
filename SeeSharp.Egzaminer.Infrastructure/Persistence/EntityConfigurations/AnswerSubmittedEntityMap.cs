using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using SeeSharp.Egzaminer.Domain.Entities;

namespace SeeSharp.Egzaminer.Infrastructure.Persistence.EntityConfigurations
{
    public class AnswerSubmittedEntityMap : IEntityTypeConfiguration<AnswerSubmitted>
    {
        public void Configure(EntityTypeBuilder<AnswerSubmitted> builder)
        {
            builder.Property(x => x.ProvidedAnswer).IsRequired(false);
            builder.Property(x => x.SelectedOptions).IsRequired(false);
            builder.Property(x => x.AutoScore).IsRequired();
            builder.Property(x => x.ManualScore).IsRequired();
            builder.Property(x => x.ManualComment).IsRequired(false);

            builder.HasOne(x => x.TestSubmission)
                .WithMany(s => s.Answers)
                .HasForeignKey(x => x.TestSubmissionId)
                .IsRequired();

            builder.HasOne(x => x.Question)
                .WithMany()
                .HasForeignKey(x => x.QuestionId)
                .IsRequired(false);

            // Ustawiamy DeleteBehavior.NoAction w tej relacji, aby uniknąć cykli
            builder.HasOne(x => x.TestPublication)
                .WithMany(tp => tp.AnswerSubmitted)  // Zakładając, że w TestPublication masz kolekcję AnswerSubmitted
                .HasForeignKey(x => x.TestPublicationId)
                .OnDelete(DeleteBehavior.NoAction);  // Można także użyć DeleteBehavior.SetNull
        }
    }
}
