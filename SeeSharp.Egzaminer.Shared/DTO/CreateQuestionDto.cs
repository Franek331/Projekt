namespace SeeSharp.Egzaminer.Shared.DTO;

public class CreateQuestionDto
{
    public string Content { get; set; } = default!;
    public int QuestionType { get; set; } // SingleChoice=0, MultiChoice=1...
    public decimal Points { get; set; }
    public List<CreateQuestionAnswerDto> Answers { get; set; } = new();
}