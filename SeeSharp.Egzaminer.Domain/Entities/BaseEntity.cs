using System.ComponentModel.DataAnnotations.Schema;

namespace SeeSharp.Egzaminer.Domain.Entities;

public abstract class BaseEntity<TKey> where TKey : struct
{
    [Column(nameof(Id), Order = 0)]
    public TKey Id { get; set; }
}

public abstract class BaseEntity : BaseEntity<Guid>
{
}
