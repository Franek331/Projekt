using SeeSharp.Egzaminer.Domain.Entities;

namespace SeeSharp.Egzaminer.Shared.DTO;

public class PublishTestDto
{
    public Guid TestId { get; set; }
    public DateTime StartDate { get; set; }
    public DateTime? EndDate { get; set; }
    public int? MaxAttempts { get; set; }
    public TestPublicationStatuses Status { get; set; } = TestPublicationStatuses.Published;
    public TestDto Test { get; set; } = default!;
}

