using SeeSharp.Egzaminer.Shared.DTO;
using SeeSharp.Egzaminer.Shared.Wrapper;
using System.Net.Http.Json;

namespace SeeSharp.Egzaminer.Client.Services
{
    public class TestProxyService : ITestProxyService
    {
        private readonly HttpClient httpClient;

        public TestProxyService(HttpClient httpClient)
        {
            this.httpClient = httpClient;
        }

        // Pobieranie wszystkich testów
        public async Task<IEnumerable<PublishTestDto>> GetAll()
        {
            try
            {
                // Zwraca dane testów w opakowaniu Result
                Result<IEnumerable<PublishTestDto>> tests = await httpClient.GetFromJsonAsync<Result<IEnumerable<PublishTestDto>>>("/api/Tests");
                return tests?.Data ?? Enumerable.Empty<PublishTestDto>();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd przy pobieraniu testów: {ex.Message}");
                return Enumerable.Empty<PublishTestDto>(); 
            }
        }

        // Aktualizacja testu
        public async Task Update(PublishTestDto test)
        {
            try
            {
                await httpClient.PutAsJsonAsync("api/Tests", test);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd przy aktualizacji testu: {ex.Message}");
            }
        }

        // Sprawdzenie, czy test ma co najmniej jedno pytanie
        public async Task<bool> CheckTestHaveOneQuestion(Guid id)
        {
            try
            {
                bool result = await httpClient.GetFromJsonAsync<bool>($"api/Tests/checkhaveoneormorequestion/{id}");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd przy sprawdzaniu pytań w teście: {ex.Message}");
                return false;
            }
        }

        // Sprawdzenie, czy pytanie ma co najmniej jedną poprawną odpowiedź
        public async Task<bool> CheckQuestionHaveOneOrMoreCorrectAnswer(Guid id)
        {
            try
            {
                bool result = await httpClient.GetFromJsonAsync<bool>($"api/Tests/checkquestionhavecorrectanswer/{id}");
                return result;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd przy sprawdzaniu odpowiedzi: {ex.Message}");
                return false;
            }
        }

        // Pobranie liczby odpowiedzi oczekujących na ocenę dla konkretnego testu
        public async Task<int> GetPendingGradingCount(Guid testId)
        {
            try
            {
                var response = await httpClient.GetFromJsonAsync<int>($"api/Tests/{testId}/pendinggradingcount");
                return response;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd przy pobieraniu liczby oczekujących ocen: {ex.Message}");
                return 0;
            }
        }

        // Pobranie całkowitej liczby odpowiedzi oczekujących na ocenę (dla wszystkich testów)
        public async Task<int> GetTestsPendingGradingCount()
        {
            try
            {
                var response = await httpClient.GetFromJsonAsync<Result<int>>("/api/Tests/get-pending-grading-count");
                return response?.Data ?? 0;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Błąd przy pobieraniu liczby oczekujących ocen dla wszystkich testów: {ex.Message}");
                return 0;
            }
        }
    }

    public interface ITestProxyService
    {
        Task<IEnumerable<PublishTestDto>> GetAll();
        Task Update(PublishTestDto test);
        Task<bool> CheckTestHaveOneQuestion(Guid id);
        Task<bool> CheckQuestionHaveOneOrMoreCorrectAnswer(Guid id);
        Task<int> GetPendingGradingCount(Guid testId);
        Task<int> GetTestsPendingGradingCount();
    }
}
