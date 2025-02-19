namespace SeeSharp.Egzaminer.Shared.DTO;
public class CreateTestDto
{
    public string Title { get; set; } = default!;
    public string? Description { get; set; }
    // Lista pytań? Lub inny minimalny zestaw
}