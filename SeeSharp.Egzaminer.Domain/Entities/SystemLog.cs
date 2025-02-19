namespace SeeSharp.Egzaminer.Domain.Entities;

public class SystemLog : BaseEntity
{
    public DateTime LogDate { get; set; }
    public LogLevels LogLevel { get; set; } = LogLevels.Info; // Info, Warning, Error
    public string Message { get; set; } = default!;
    public Guid? UserId { get; set; }
}

public enum LogLevels
{
    Info,
    Warning,
    Error
}