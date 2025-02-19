namespace SeeSharp.Egzaminer.Domain.Entities;

public class AccessToken : BaseEntity
{
    public Guid TestPublicationId { get; set; }
    public TestPublication TestPublication { get; set; } = default!;

    // Kto otrzymał token (opcjonalnie)
    public string? UserId { get; set; }
    public ApplicationUser? User { get; set; }

    // Właściwy token (np. unikatowy GUID/string do linku)
    public string TokenValue { get; set; } = default!;

    // Ile podejść już wykorzystano
    public int AttemptsUsed { get; set; }
}