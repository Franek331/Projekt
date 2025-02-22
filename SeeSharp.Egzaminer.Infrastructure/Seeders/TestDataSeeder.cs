using System;
using System.Linq;
using SeeSharp.Egzaminer.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace SeeSharp.Egzaminer.Infrastructure.Persistence
{
    public class TestDataSeeder
    {
        public static void Seed(AppDbContext context)
        {
            // Upewnij się, że tabela Tests ma dane
            if (!context.Set<Test>().Any())
            {
                var test1 = new Test
                {
                    Id = Guid.NewGuid(),
                    Title = "Test 1",
                    Description = "Opis testu 1"
                };

                var test2 = new Test
                {
                    Id = Guid.NewGuid(),
                    Title = "Test 2",
                    Description = "Opis testu 2"
                };

                context.Set<Test>().AddRange(test1, test2);
                context.SaveChanges();
            }

            // Upewnij się, że tabela Questions ma dane
            if (!context.Set<Question>().Any())
            {
                var question = new Question
                {
                    Id = Guid.NewGuid(),
                    Content = "Przykładowa treść pytania"
                };
                context.Set<Question>().Add(question);
                context.SaveChanges();
            }

            // Dodaj dane do TestPublications, jeśli nie istnieją
            if (!context.TestPublications.Any())
            {
                // Pobierz istniejące testy
                var test1FromDb = context.Set<Test>().First();
                var test2FromDb = context.Set<Test>().Skip(1).First();

                var testPublication1 = new TestPublication
                {
                    Id = Guid.NewGuid(),
                    TestId = test1FromDb.Id, // Używamy istniejącego TestId
                    StartDate = DateTime.UtcNow,
                    EndDate = DateTime.UtcNow.AddDays(30),
                    MaxAttempts = 3,
                    Status = TestPublicationStatuses.Published
                };

                var testPublication2 = new TestPublication
                {
                    Id = Guid.NewGuid(),
                    TestId = test2FromDb.Id, // Używamy istniejącego TestId
                    StartDate = DateTime.UtcNow.AddDays(1),
                    EndDate = DateTime.UtcNow.AddDays(31),
                    MaxAttempts = 2,
                    Status = TestPublicationStatuses.Created
                };

                context.TestPublications.AddRange(testPublication1, testPublication2);
                context.SaveChanges();

                // Teraz dodaj TestSubmission, przypisując istniejący TestPublicationId (np. testPublication1)
                var testSubmission = new TestSubmission
                {
                    Id = Guid.NewGuid(),
                    UserId = Guid.NewGuid(), // lub konkretny istniejący UserId, jeśli masz
                    SubmissionDate = DateTime.UtcNow,
                    TestPublicationId = testPublication1.Id // Ważne: Używamy istniejącego TestPublicationId
                };
                context.Set<TestSubmission>().Add(testSubmission);
                context.SaveChanges();

                // Pobierz istniejący TestSubmission oraz Question
                var testSubmissionFromDb = context.Set<TestSubmission>().First();
                var questionFromDb = context.Set<Question>().First();

                // Dodaj dane do AnswerSubmitted, powiązując je z TestPublication, TestSubmission oraz Question
                var answerSubmitted1 = new AnswerSubmitted
                {
                    Id = Guid.NewGuid(),
                    TestPublicationId = testPublication1.Id,
                    AutoScore = 80,
                    ManualScore = 85,
                    ProvidedAnswer = "Answer 1",
                    SelectedOptions = "Option1, Option2",
                    TestSubmissionId = testSubmissionFromDb.Id,
                    QuestionId = questionFromDb.Id
                };

                var answerSubmitted2 = new AnswerSubmitted
                {
                    Id = Guid.NewGuid(),
                    TestPublicationId = testPublication2.Id,
                    AutoScore = 70,
                    ManualScore = 75,
                    ProvidedAnswer = "Answer 2",
                    SelectedOptions = "Option2, Option3",
                    TestSubmissionId = testSubmissionFromDb.Id,
                    QuestionId = questionFromDb.Id
                };

                context.AnswerSubmitted.AddRange(answerSubmitted1, answerSubmitted2);
                context.SaveChanges();
            }
        }
    }
}
