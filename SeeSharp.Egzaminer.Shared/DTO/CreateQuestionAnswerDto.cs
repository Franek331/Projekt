namespace SeeSharp.Egzaminer.Shared.DTO;

public class CreateQuestionAnswerDto
{
    public string Content { get; set; } = default!;
    public bool IsCorrect { get; set; }
}