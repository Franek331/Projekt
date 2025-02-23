using Microsoft.EntityFrameworkCore;
using SeeSharp.Egzaminer.Application.Interfaces;
using SeeSharp.Egzaminer.Domain.Entities;
using SeeSharp.Egzaminer.Shared.DTO;
using SeeSharp.Egzaminer.Shared.Wrapper;


namespace SeeSharp.Egzaminer.Api.Controllers;

public class TestService : ITestService
{

    //private readonly AppDbContext dbContext;  //zostawione do celów edukacyjnych
    private readonly IKeyRepository<TestPublication, Guid> testRepository;   //typ encji czyli tabela w bazie danych
    List<PublishTestDto> tests;
    List<PublishTestDto> checkTests;



    //public TestService(AppDbContext dbContext)   //zostawione do celów edukacyjnych
    public TestService(IKeyRepository<TestPublication, Guid> testRepository)
    {
        //this.dbContext = dbContext;     //zostawione do celów edukacyjnych
        this.testRepository = testRepository;

        tests = new List<PublishTestDto>();
    }



    public TestService()
    {

    }


    public IEnumerable<PublishTestDto> GetAll()
    {
        //using AppDbContext dbContext = new(); na new zgłąszał bład

        tests = testRepository.GetAll()
         .Include(tp => tp.Test)
                               .ThenInclude(t => t.Questions) // Dołącz pytania
                               .Select(tp => new PublishTestDto
                               {
                                   TestId = tp.TestId,
                                   StartDate = tp.StartDate,
                                   EndDate = tp.EndDate,
                                   MaxAttempts = tp.MaxAttempts,
                                   Status = tp.Status,
                                   Test = tp.Test != null ? new TestDto
                                   {
                                       Title = tp.Test.Title,
                                       Description = tp.Test.Description,
                                       //Questions = tp.Test.Questions?.Select(q => new QuestionDto)
                                       //{
                                       //    Content = q.Content,
                                       //    Answers = q.Answers?.Select(a => new AnswerDto
                                       //    {
                                       //        Content = a.Content,
                                       //        IsCorrect = a.IsCorrect
                                       //    })
                                       //}
                                       
                                   } : null
                               })
                               .ToList();
        Console.WriteLine($"Loaded {tests.Count} tests from database");
        return tests;
    }



    public void Add(PublishTestDto testPublication)
    {
        if (tests == null)
        {
            tests = new();
        }
        tests.Add(testPublication);
    }

    public PublishTestDto? GetById(Guid id)
    {

        //var testPublication = dbContext.TestPublications   //zostawione do celów edukacyjnych
        var testPublication = testRepository.GetAll()
        .Include(tp => tp.Test)
        .FirstOrDefault(tp => tp.TestId == id);

        if (testPublication == null)
        {
            return null;
        }

        return new PublishTestDto
        {
            TestId = testPublication.TestId,
            StartDate = testPublication.StartDate,
            EndDate = testPublication.EndDate,
            MaxAttempts = testPublication.MaxAttempts,
            Status = testPublication.Status,
            Test = testPublication.Test != null ? new TestDto
            {
                Title = testPublication.Test.Title,
                Description = testPublication.Test.Description
            } : null
        };
    }



    public void Update(PublishTestDto testPublication)
    {

        var existingTestPublication = testRepository.GetAll()
            .Include(tp => tp.Test)
        .FirstOrDefault(tp => tp.TestId == testPublication.TestId);

        if (existingTestPublication != null)
        {
            existingTestPublication.MaxAttempts = testPublication.MaxAttempts;
            existingTestPublication.StartDate = testPublication.StartDate;
            existingTestPublication.EndDate = testPublication.EndDate;
            existingTestPublication.Status = testPublication.Status;

            if (testPublication.Test.Title != "" || testPublication.Test.Title != null)
                existingTestPublication.Test.Title = testPublication.Test.Title;
            if (testPublication.Test.Description != "" || testPublication.Test.Description != null)
                existingTestPublication.Test.Description = testPublication.Test.Description;

            //testRepository.SaveChanges();  //zostawione do celów edukacyjnych
            testRepository.Update(existingTestPublication);
        }
    }


    public bool CheckIdTest(Guid id)
    {
        //return dbContext.TestPublications.Any(tp => tp.TestId == id);  //zostawione do celów edukacyjnych
        return testRepository.GetAll().Any(tp => tp.TestId == id);
    }



    public async Task<bool> CheckIfTestByIdHaveQuestions(Guid id)
    {
        var testPublication = await testRepository.GetAll()
            .Include(tp => tp.Test)
            .ThenInclude(t => t.Questions)
            .FirstOrDefaultAsync(tp => tp.TestId == id);

        return testPublication?.Test?.Questions?.Any() ?? false;
    }


    public bool CheckIfQuestionsHaveMinOneAnserCorrect(Guid id)
    {
        var testPublication = testRepository.GetAll()
            .Include(tp => tp.Test)
            .ThenInclude(t => t.Questions)
            .ThenInclude(q => q.Answers)
            .FirstOrDefault(tp => tp.TestId == id);

        if (testPublication?.Test?.Questions == null || testPublication?.Test?.Questions.Count == 0)
        {
            return false;
        }

        //foreach (var question in testPublication.Test.Questions)   //działąjacy kod zostawiony do celów edukacyjnych
        //{
        //    if (question.Answers == null || !question.Answers.Any(a => a.IsCorrect))
        //    {
        //        return false;
        //    }
        //}
        //return true;

        return testPublication.Test.Questions.All(question => question.Answers != null && question.Answers.Any(a => a.IsCorrect));

        
    }

    //public void Delete(int id)    //zostatwione do celów edukacyjnych
    //{
    //    BookDto book = GetById(id);
    //    if (book != null)
    //    {
    //    books.Remove(book);
    //    }
    //}
    public async Task<int> GetTestsPendingGradingCount()
    {
        return await testRepository.GetPendingGradingCount();
    }
}


public interface ITestService
{
    IEnumerable<PublishTestDto> GetAll();
    PublishTestDto? GetById(Guid id);
    void Add(PublishTestDto testPublication);
    void Update(PublishTestDto testPublication);
    bool CheckIdTest(Guid id);
    //bool CheckIfTestByIdHaveQuestions(Guid id);
    Task<bool> CheckIfTestByIdHaveQuestions(Guid id);
    bool CheckIfQuestionsHaveMinOneAnserCorrect(Guid id);
    Task<int> GetTestsPendingGradingCount();


    //void Delete(int id);


}