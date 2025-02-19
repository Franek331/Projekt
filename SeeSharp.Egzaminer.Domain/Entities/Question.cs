namespace SeeSharp.Egzaminer.Domain.Entities;
public enum QuestionType
{
    SingleChoice,
    MultipleChoice,
    TrueFalse,
    Open
    // Możesz dodać więcej
}

public class Question : BaseEntity
{
    public string Content { get; set; } = default!;
    public QuestionType QuestionType { get; set; }

    // Maksymalna liczba punktów za to pytanie
    public decimal Points { get; set; } = 1;

    // Ewentualnie: Tagowanie, relacje do Testu, itp.
    public ICollection<QuestionAnswer>? Answers { get; set; } = new List<QuestionAnswer>();
    public ICollection<Test> Tests { get; set; } = new List<Test>();
}