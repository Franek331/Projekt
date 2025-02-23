// ManualGradingResultEntityMap.cs
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using SeeSharp.Egzaminer.Domain.Entities;

namespace SeeSharp.Egzaminer.Infrastructure.Persistence.EntityConfigurations
{
    public class ManualGradingResultEntityMap : IEntityTypeConfiguration<ManualGradingResult>
    {
        public void Configure(EntityTypeBuilder<ManualGradingResult> builder)
        {
            builder.ToTable("ManualGradingResults");

            builder.HasKey(mg => mg.Id);

            builder.HasOne(mg => mg.AnswerSubmitted)
                // Kolekcja jest określona w AnswerSubmittedEntityMap, więc wystarczy WithOne, jeżeli AnswerSubmitted posiada pojedynczy ManualGradingResult lub WithMany, jeśli kolekcja
                .WithMany(asub => asub.ManualGradingResults)
                .HasForeignKey(mg => mg.AnswerSubmittedId)
                .OnDelete(DeleteBehavior.Cascade);

            builder.Property(mg => mg.Points)
                .IsRequired()
                .HasColumnType("decimal(18,2)");

            builder.Property(mg => mg.Comment)
                .HasMaxLength(500);

            builder.Property(mg => mg.GradedDate)
                .IsRequired();
        }
    }
}
