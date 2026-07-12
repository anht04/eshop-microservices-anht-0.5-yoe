using BuildingBlocks.DDD.Abstractions;

namespace BuildingBlocks.DDD.Extensions;

public static class SoftDeletableExtensions
{
    public static void MarkAsDeleted(this ISoftDeletable entity, string? deletedBy = null)
    {
        if (entity.IsDeleted) return;

        entity.IsDeleted = true;
        entity.DeletedAt = DateTimeOffset.UtcNow;
        entity.DeletedBy = deletedBy;
    }

    public static void Restore(this ISoftDeletable entity, string? restoredBy = null)
    {
        if (!entity.IsDeleted) return;

        entity.IsDeleted = false;
        entity.DeletedAt = null;
        entity.DeletedBy = restoredBy;
    }
}