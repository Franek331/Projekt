namespace SeeSharp.Egzaminer.Domain.Entities;

public class TestSubmission : BaseEntity
{
    public Guid TestPublicationId { get; set; }
    public TestPublication TestPublication { get; set; } = default!;

    public Guid UserId { get; set; }
    public ApplicationUser User { get; set; } = default!;

    public DateTime StartTime { get; set; }
    public DateTime? EndTime { get; set; }

    // Suma punktów obliczona automatycznie
    public decimal AutoScore { get; set; }
    // Suma punktów z oceny ręcznej
    public decimal ManualScore { get; set; }
    // Suma łączna
    public decimal TotalScore { get; set; }

    public ICollection<AnswerSubmitted> Answers { get; set; } = new List<AnswerSubmitted>();
}