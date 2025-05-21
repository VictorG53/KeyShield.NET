using Microsoft.EntityFrameworkCore.Diagnostics;
using KeyShieldDB.Interfaces;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore;

namespace KeyShieldDB.Interceptors;

public sealed class SoftDeleteInterceptor : SaveChangesInterceptor
{
    public override ValueTask<InterceptionResult<int>> SavingChangesAsync(
            DbContextEventData eventData,
            InterceptionResult<int> result,
            CancellationToken cancellationToken = default)
    {
        if (eventData.Context is null)
        {
            return base.SavingChangesAsync(
                eventData, result, cancellationToken);
        }

        IEnumerable<EntityEntry<ISoftDelete>> entries =
            eventData
                .Context
                .ChangeTracker
                .Entries<ISoftDelete>()
                .Where(e => e.State == EntityState.Deleted);

        foreach (EntityEntry<ISoftDelete> softDeletable in entries)
        {
            softDeletable.State = EntityState.Modified;
            softDeletable.Entity.DateSuppression = DateTime.UtcNow;
        }

        return base.SavingChangesAsync(eventData, result, cancellationToken);
    }

}