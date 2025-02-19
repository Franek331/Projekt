using Microsoft.AspNetCore.Components;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using MudBlazor;
using MudBlazor.Extensions;
using SeeSharp.Egzaminer.Client.Services;
using SeeSharp.Egzaminer.Domain.Entities;
using SeeSharp.Egzaminer.Shared.DTO;

namespace SeeSharp.Egzaminer.Client.Pages
{
    public partial class DetailsTest
    {

        [Parameter]
        public string id { get; set; }

        public string IdUpper { get; set; }

        public string TitleTest { get; set; }

        public string Description { get; set; }


        bool AreQuestion = false;
        bool AreCorrectAnswer = false;


        [Inject]
        public NavigationManager Navigation { get; set; }
        [Inject]
        public ISnackbar Snackbar { get; set; }







        public List<PublishTestDto> TestsList { get; set; } = new();

        [Inject]
        public ITestProxyService TestService { get; set; }


        //mudblazor
        private TimeSpan? _time = new TimeSpan(00, 45, 00);
        private TimeSpan? _time2 = new TimeSpan(00, 45, 00);
        public bool EndDateTest_CheckBox { get; set; } = true;
        public int IntValue { get; set; }
        private DateTime? selectedStartDate;
        //private DateTime? selectedEndDate;

        private DateTime? selectedEndDate;
        protected override async Task OnInitializedAsync()
        {
            IdUpper = id.ToUpper();
            await GetTestListAll();
            TitleTest = TestsList.FirstOrDefault(test => test.TestId.ToString().ToUpper() == IdUpper)?.Test.Title;
            Description = TestsList.FirstOrDefault(test => test.TestId.ToString().ToUpper() == IdUpper)?.Test.Description;
            selectedStartDate = TestsList.FirstOrDefault(test => test.TestId.ToString().ToUpper() == IdUpper)?.StartDate;
            selectedEndDate = TestsList.FirstOrDefault(test => test.TestId.ToString().ToUpper() == IdUpper)?.EndDate;
            IntValue = TestsList.FirstOrDefault(test => test.TestId.ToString().ToUpper() == IdUpper)?.MaxAttempts ?? 1;
            _time = TestsList.FirstOrDefault(test => test.TestId.ToString().ToUpper() == IdUpper)?.StartDate.TimeOfDay;
            DateTime? _dateEndDB = TestsList.FirstOrDefault(test => test.TestId.ToString().ToUpper() == IdUpper)?.EndDate ?? null;
            if (_dateEndDB != null)
                _time2 = _dateEndDB.Value.TimeOfDay;

            if (selectedEndDate != null)
            {
                EndDateTest_CheckBox = true;
            }
            else
            {
                EndDateTest_CheckBox = false;
            }

            await OnValidation();

            StateHasChanged();
        }

        private async Task GetTestListAll()
        {

            try
            {
                TestsList = (await TestService.GetAll())?.ToList() ?? [];
            }
            catch
            {
                TestsList = new();
            }



        }

        private async Task OnUpdateTest()
        {



            DateTime selectedStartDate2 = new DateTime();
            DateTime selectedEndDate2 = new DateTime();

            if (selectedEndDate.HasValue && _time2.HasValue)
            {
                selectedEndDate2 = selectedEndDate.Value.Date + _time2.Value;
            }
            if (selectedStartDate.HasValue && _time.HasValue)
            {
                selectedStartDate2 = selectedStartDate.Value.Date + _time.Value;
            }

            PublishTestDto test = new PublishTestDto
            {
                TestId = Guid.Parse(id),
                StartDate = selectedStartDate2,
                EndDate = EndDateTest_CheckBox ? selectedEndDate2 : null,
                MaxAttempts = IntValue,
                Status = TestPublicationStatuses.Published,
                Test = new TestDto
                {
                    Title = TitleTest,
                    Description = Description,
                }
            };

            await TestService.Update(test);
            Navigation.NavigateTo("/");
            Snackbar.Add("Zmiany zosta³y zapisane.", Severity.Success);
            //return Task.CompletedTask;
        }
        private async Task onPublicationClick()
        {
            await OnUpdateTest();
            StateHasChanged();
        }


        private async Task OnValidation()
        {
            try
            {
                AreQuestion = (await TestService.CheckTestHaveOneQuestion(Guid.Parse(id)));
            }
            catch
            {
                AreQuestion = false;
            }
            try
            {
                AreCorrectAnswer = (await TestService.CheckQuestionHaveOneOrMoreCorrectAnswer(Guid.Parse(id)));
            }
            catch
            {
                AreCorrectAnswer = false;
            }

        }


    }
}