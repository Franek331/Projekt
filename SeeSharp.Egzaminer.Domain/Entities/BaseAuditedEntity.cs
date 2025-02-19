namespace SeeSharp.Egzaminer.Domain.Entities;
public abstract class BaseAuditedEntity : BaseEntity
{
    public DateTime CreateAt { get; set; }

    public DateTime ModifiedAt { get; set; }
}
