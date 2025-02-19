using MudBlazor;
using static MudBlazor.CategoryTypes;
using System.Net.Http.Json;
using System.Net.Http.Json;
//using MudBlazor.Examples.Data.Models;
using System.Threading;
using Microsoft.AspNetCore.Components;
using System.Text.Json.Serialization;
using SeeSharp.Egzaminer.Shared.DTO;
using SeeSharp.Egzaminer.Client.Services;
using System.Collections.Generic;
using SeeSharp.Egzaminer.Domain.Entities;
using Microsoft.EntityFrameworkCore.Storage.Internal;
using Microsoft.EntityFrameworkCore.Metadata.Internal;

namespace SeeSharp.Egzaminer.Client.Pages
{

    public partial class PanelTeacher
    {

        [Inject]
        public ITestProxyService TestService { get; set; }
        [Inject]
        public NavigationManager Navigation { get; set; }

        private IEnumerable<Element> pagedData;
        private MudTable<Element> table;
        private int totalItems;
        private string searchString = null;

        public int Number { get; set; } = 0;
        public List<PublishTestDto> TestsList { get; set; } = new();

        private async Task GetData()
        {
            try
            {
                TestsList = (await TestService.GetAll())?.ToList() ?? new();
            }
            catch
            {
                TestsList = new();
            }
        }

        private async Task<TableData<Element>> ServerReload(TableState state, CancellationToken token)
        {
            await GetData();
            IEnumerable<Element> data = await Task.WhenAll(TestsList.Select(async x => await TransformToElementAsync(x)));

            data = data.Where(element =>
            {
                if (string.IsNullOrWhiteSpace(searchString))
                    return true;
                if (element.Name.Contains(searchString, StringComparison.OrdinalIgnoreCase))
                    return true;
                if ($"{element.Number} {element.Name}".Contains(searchString))
                    return true;
                return false;
            }).ToArray();

            totalItems = data.Count();
            switch (state.SortLabel)
            {
                case "nr_field":
                    Number = 0;
                    data = data.OrderByDirection(state.SortDirection, o => o.Number);
                    break;
                case "status_field":
                    Number = 0;
                    data = data.OrderByDirection(state.SortDirection, o => o.Status);
                    break;
                case "name_field":
                    Number = 0;
                    data = data.OrderByDirection(state.SortDirection, o => o.Name);
                    break;
            }

            pagedData = data.Skip(state.Page * state.PageSize).Take(state.PageSize).ToArray();
            return new TableData<Element>() { TotalItems = totalItems, Items = pagedData };
        }

        private void OnSearch(string text)
        {
            searchString = text;
            table.ReloadServerData();
        }

        private async Task<Element> TransformToElementAsync(PublishTestDto dto)
        {
            Number++;

            // Zamieniamy DTO na element tabeli, dodajemy licznik odpowiedzi oczekuj¹cych na ocenê
            int pendingGradingCount = await TestService.GetPendingGradingCount(dto.TestId);

            return new Element
            {
                Number = this.Number,
                TestId = dto.TestId,
                Name = dto.Test.Title,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                MaxAttempts = dto.MaxAttempts,
                Status = dto.Status,
                PendingGradingCount = pendingGradingCount
            };
        }

        private async Task OnPublicationClick(Guid id)
        {
            // Przejœcie do widoku szczegó³owego testu z odpowiedziami do oceniania
            Navigation.NavigateTo($"/test-grading/{id}");
        }

        public class Element
        {
            public string Group { get; set; }
            public Guid TestId { get; set; }
            public int Number { get; set; }
            public string Name { get; set; }
            public DateTime StartDate { get; set; }
            public DateTime? EndDate { get; set; }
            public int? MaxAttempts { get; set; }
            public TestPublicationStatuses Status { get; set; }
            public int PendingGradingCount { get; set; }

            public override string ToString()
            {
                return $"{Name} - {Status}";
            }
        }



    }
}