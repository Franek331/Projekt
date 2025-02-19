using SeeSharp.Egzaminer.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System;

namespace SeeSharp.Egzaminer.Infrastructure.Persistence
{
    public class TestDataSeeder
    {
        public static void Seed(AppDbContext context)
        {
            // Sprawdzamy, czy TestPublications już istnieją
            if (!context.TestPublications.Any())
            {
                // Dodaj dane TestPublication
                var testPublication1 = new TestPublication
                {
                    Id = Guid.NewGuid(), // Unikalny ID
                    TestId = Guid.NewGuid(), // TestId, może być również unikalne
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddDays(30),
                    MaxAttempts = 3,
                    Status = TestPublicationStatuses.Published
                };

                var testPublication2 = new TestPublication
                {
                    Id = Guid.NewGuid(),
                    TestId = Guid.NewGuid(),
                    StartDate = DateTime.UtcNow.AddDays(1),
                    EndDate = DateTime.UtcNow.AddDays(31),
                    MaxAttempts = 2,
                    Status = TestPublicationStatuses.Created
                };

                context.TestPublications.AddRange(testPublication1, testPublication2);

                // Dodaj dane AnswerSubmitted
                var answerSubmitted1 = new AnswerSubmitted
                {
                    Id = Guid.NewGuid(),
                    TestPublicationId = testPublication1.Id, // Powiązanie z testPublication1
                    AutoScore = 80,
                    ManualScore = 85,
                    ProvidedAnswer = "Answer 1",
                    SelectedOptions = "Option1, Option2",
                    TestSubmissionId = Guid.NewGuid() // Jeśli TestSubmissionId jest wymagane
                };

                var answerSubmitted2 = new AnswerSubmitted
                {
                    Id = Guid.NewGuid(),
                    TestPublicationId = testPublication2.Id, 
                    AutoScore = 70,
                    ManualScore = 75,
                    ProvidedAnswer = "Answer 2",
                    SelectedOptions = "Option2, Option3",
                    TestSubmissionId = Guid.NewGuid() // Jeśli TestSubmissionId jest wymagane
                };

                context.AnswerSubmitted.AddRange(answerSubmitted1, answerSubmitted2);

                // Zapisz zmiany w bazie danych
                context.SaveChanges();
            }
        }
    }
}
