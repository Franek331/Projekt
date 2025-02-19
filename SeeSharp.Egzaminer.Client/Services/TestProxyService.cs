using SeeSharp.Egzaminer.Shared.DTO;
using SeeSharp.Egzaminer.Shared.Wrapper;
using System.Net.Http.Json;


namespace SeeSharp.Egzaminer.Client.Services;

public class TestProxyService : ITestProxyService
{
    private readonly HttpClient httpClient;

    public TestProxyService(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }

    public async Task<IEnumerable<PublishTestDto>> GetAll()
    {
        Result<IEnumerable<PublishTestDto>> tests = await httpClient.GetFromJsonAsync<Result<IEnumerable<PublishTestDto>>>("/api/Tests");
        return (IEnumerable<PublishTestDto>)tests.Data;
    }

    public async Task Update(PublishTestDto test)
    {
        await httpClient.PutAsJsonAsync("api/Tests", test);
    }

    public async Task<bool> CheckTestHaveOneQuestion(Guid id)
    {
        bool result = await httpClient.GetFromJsonAsync<bool>($"api/Tests/checkhaveoneormorequestion/{id}");
        return result;
    }

    public async Task<bool> CheckQuestionHaveOneOrMoreCorrectAnswer(Guid id)
    {
        bool result = await httpClient.GetFromJsonAsync<bool>($"api/Tests/checkquestionhavecorrectanswer/{id}");
        return result;
    }

    // Implementacja metody GetPendingGradingCount
    public async Task<int> GetPendingGradingCount(Guid testId)
    {
        // Zakładając, że API zwraca liczbę oczekujących ocen (np. zlicza odpowiedzi, które nie mają przypisanych punktów)
        var response = await httpClient.GetFromJsonAsync<int>($"api/Tests/{testId}/pendinggradingcount");
        return response;
    }

    public async Task<int> GetTestsPendingGradingCount()
    {
        var response = await httpClient.GetFromJsonAsync<Result<int>>("/api/Tests/get-pending-grading-count");
        return response?.Data ?? 0;
    }
}

public interface ITestProxyService
{
    Task<IEnumerable<PublishTestDto>> GetAll();
    Task Update(PublishTestDto test);
    Task<bool> CheckTestHaveOneQuestion(Guid id);
    Task<bool> CheckQuestionHaveOneOrMoreCorrectAnswer(Guid id);

    // Dodajemy nową metodę
    Task<int> GetPendingGradingCount(Guid testId);
}
