using System.Security.Claims;
using BuildingBlocks.DDD.Abstractions;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.Diagnostics;

namespace Ordering.Infrastructure.Data.Interceptors;

public class AuditableEntityInterceptor(IHttpContextAccessor httpContextAccessor) : SaveChangesInterceptor
{
    public override InterceptionResult<int> SavingChanges(DbContextEventData eventData, InterceptionResult<int> result)
    {
        UpdateEntities(eventData.Context);
        
        return base.SavingChanges(eventData, result);
    }

    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = new ())
    {
        UpdateEntities(eventData.Context);
        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

    private void UpdateEntities(DbContext? context)
    {
        if (context is null)
        {
            return;
        }
        var currentUser = GetCurrentUser();

        foreach (var entry in context.ChangeTracker.Entries<IEntity>()) 
        {
            if (entry.State == EntityState.Added && entry.Entity is IHasCreationAudit creationAudit)
            {
                creationAudit.CreatedBy = currentUser;
                creationAudit.CreatedAt = DateTime.Now;
            }

            if ((entry.State == EntityState.Added || 
                entry.State == EntityState.Modified || 
                entry.HasChangedOwnedEntities()) &&
                entry.Entity is IHasModificationAudit modificationAudit)
            {
                modificationAudit.LastModifiedBy = currentUser;
                modificationAudit.LastModified = DateTime.UtcNow;
            }
        }
    }
    
    private string GetCurrentUser()
    {
        var httpContext = httpContextAccessor.HttpContext;

        if (httpContext?.User.Identity?.IsAuthenticated != true)
        {
            return "System";
        }

        var subjectRef = httpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        var sub = httpContext.User.FindFirst("sub")?.Value;

        return subjectRef ?? sub ?? "Unknown User";
    }
}

public static class Extensions
{
    public static bool HasChangedOwnedEntities(this EntityEntry entry) =>
        entry.References.Any(r => 
            r.TargetEntry != null && 
            r.TargetEntry.Metadata.IsOwned() && 
            r.TargetEntry.State is EntityState.Added or EntityState.Modified);
}