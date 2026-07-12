using System.ComponentModel.DataAnnotations;
using BuildingBlocks.DDD.Abstractions;

namespace BuildingBlocks.DDD;

public abstract class AuditableEntity<T> : IEntity<T>, IHasCreationAudit, IHasModificationAudit
{
    [Key]
    public T Id { get; init; }

    public DateTimeOffset CreatedAt { get; set; } = DateTimeOffset.UtcNow;

    public string? CreatedBy { get; set; }

    public DateTimeOffset? LastModified { get; set; }

    public string? LastModifiedBy { get; set; }
}