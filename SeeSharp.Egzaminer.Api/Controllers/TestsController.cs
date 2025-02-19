using Microsoft.AspNetCore.Mvc;
using SeeSharp.Egzaminer.Shared.DTO;
using SeeSharp.Egzaminer.Shared.Wrapper;

namespace SeeSharp.Egzaminer.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestsController : ControllerBase
{
    private readonly ITestService testService;

    public TestsController(ITestService testService)
    {
        this.testService = testService;
    }   
  

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        //return Ok(testService.GetAll());  //zachowałem zapis aby wiedziec jak było wcześniej a jak powinno być
        return Ok(Result<IEnumerable<PublishTestDto>>.Success(testService.GetAll()));
        
    }


    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        PublishTestDto test = testService.GetById(id);
        if (test is null)
        {
            return NotFound();
        }
        //return Ok(test); //zachowałem zapis aby wiedziec jak było wcześniej a jak powinno być
        return Ok(Result<PublishTestDto>.Success(test));
    }



    [HttpPost]
    public async Task<IActionResult> Add([FromBody] PublishTestDto test)
    {
        testService.Add(test);
        return Created($"api/details-publication/{test.TestId}", test);
    }

    [HttpPut]
    public async Task<IActionResult> Update(PublishTestDto test)
    {
        testService.Update(test);

        return NoContent();
    }


    [HttpGet("check/{id}")]
    public async Task<IActionResult> CheckIdTest(Guid id)
    {
        bool existsTest = testService.CheckIdTest(id);

        return Ok(Result<bool>.Success(existsTest));
    }


    [HttpGet("checkhaveoneormorequestion/{id}")]
    public async Task<IActionResult> CheckHaveOneOrMoreQuestion(Guid id)
    {
        bool existsTest = await testService.CheckIfTestByIdHaveQuestions(id);
        return Ok(Result<bool>.Success(existsTest));
    }

    [HttpGet("checkquestionhavecorrectanswer/{id}")]
    public async Task<IActionResult> CheckQuestionHaveCorrectAnswer(Guid id)
    {
        bool existsTest = testService.CheckIfQuestionsHaveMinOneAnserCorrect(id);

        return Ok(Result<bool>.Success(existsTest));
    }

    [HttpGet("get-pending-grading-count")]
    public async Task<IActionResult> GetTestsPendingGradingCount()
    {
        int pendingCount = await testService.GetTestsPendingGradingCount();
        return Ok(Result<int>.Success(pendingCount));
    }


    //[HttpDelete("{id}")]
    //public async Task<IActionResult> Delete(int id)
    //{
    //    bookService.Delete(id);
    //    return NoContent();
    //}


}