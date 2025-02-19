namespace SeeSharp.Egzaminer.Domain.Entities;

public class GlobalSetting : BaseEntity
{
    public string Key { get; set; } = default!;
    public string Value { get; set; } = default!;
}