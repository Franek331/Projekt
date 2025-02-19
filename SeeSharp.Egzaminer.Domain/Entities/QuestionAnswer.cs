namespace SeeSharp.Egzaminer.Domain.Entities;

public class QuestionAnswer : BaseEntity
{
    public string Content { get; set; }
    public bool IsCorrect { get; set; }

    // Powiązanie do pytania:
    public Guid QuestionId { get; set; }
    public Question Question { get; set; } = default!;
}