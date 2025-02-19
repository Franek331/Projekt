using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SeeSharp.Egzaminer.Domain.Entities;

namespace SeeSharp.Egzaminer.Infrastructure.Persistence.EntityConfigurations;

    public class ManualGradingResultEntityMap : IEntityTypeConfiguration<ManualGradingResult>
    {
        public void Configure(EntityTypeBuilder<ManualGradingResult> builder)
        {
            // Tabela dla ManualGradingResult
            builder.ToTable("ManualGradingResults");

            // Klucz główny (może pochodzić z BaseEntity, zakładając, że 'Id' jest w BaseEntity)
            builder.HasKey(mg => mg.Id);

            // Relacja z AnswerSubmitted (zakładając, że AnswerSubmitted to inna encja)
            builder.HasOne(mg => mg.AnswerSubmitted)
                .WithMany()  // Jeśli AnswerSubmitted nie zawiera kolekcji ManualGradingResults
                .HasForeignKey(mg => mg.AnswerSubmittedId)
                .OnDelete(DeleteBehavior.Cascade);  // Możesz zmienić zachowanie usuwania w zależności od wymagań

            // Właściwości
            builder.Property(mg => mg.Points)
                .IsRequired()  // Przyznane punkty są wymagane
                .HasColumnType("decimal(18,2)");  // Określamy dokładność dla typu decimal

            builder.Property(mg => mg.Comment)
                .HasMaxLength(500);  // Ustawiamy maksymalną długość komentarza (możesz dostosować)

            builder.Property(mg => mg.GradedDate)
                .IsRequired();  // Data oceniania jest wymagana
        }
    }

