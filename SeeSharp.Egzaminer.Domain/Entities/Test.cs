namespace SeeSharp.Egzaminer.Domain.Entities;
public class Test : BaseEntity
{
    public string Title { get; set; } = default!;
    public string? Description { get; set; }

    // Relacja wiele-do-wielu z pytaniami:
    public ICollection<Question> Questions { get; set; } = new List<Question>();
}