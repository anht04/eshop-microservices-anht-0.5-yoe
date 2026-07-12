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
    
    public override bool Equals(object? obj)
    {
        if (obj is not AuditableEntity<T> other)
        {
            return false;
        }

        if (ReferenceEquals(this, other))
        {
            return true;
        }

        if (Id is null || other.Id is null)
        {
            return false;
        }

        return EqualityComparer<T>.Default.Equals(Id, other.Id);
    }
    
    public override int GetHashCode() => Id?.GetHashCode() ?? 0;
    
}