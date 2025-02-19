namespace SeeSharp.Egzaminer.Domain.Entities;

// Publikowanie testu – parametry dostępności
public class TestPublication : BaseEntity
{
    public Guid TestId { get; set; }
    public Test Test { get; set; } = default!;

    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }

    public int? MaxAttempts { get; set; } // null => nieograniczone

    // e.g. Published, Cancelled, etc.
    public TestPublicationStatuses Status { get; set; } = TestPublicationStatuses.Published;

    public ICollection<AnswerSubmitted> AnswerSubmitted { get; set; } = new List<AnswerSubmitted>();
}

public enum TestPublicationStatuses
{
    Published,
    Cancelled,
    Archived,
    Created
}