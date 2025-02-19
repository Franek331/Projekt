namespace SeeSharp.Egzaminer.Domain.ValueObjects;

public class Patch
{
    public required string PropertyName { get; set; }
    public object? PropertyValue { get; set; }
}