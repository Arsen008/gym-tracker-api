using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Diagnostics;
using SachkovTech.Domain.Shared;

namespace SachkovTech.Infrastructure.Interceptors;

public class SoftDeleteInterceptor : SaveChangesInterceptor
{
    public override async ValueTask<InterceptionResult<int>> SavingChangesAsync(
        DbContextEventData eventData,
        InterceptionResult<int> result,
        CancellationToken cancellationToken = default)
    {
        if (eventData.Context is null)
        {
            return await base.SavingChangesAsync(eventData, result, cancellationToken);
        }

        var entries = eventData.Context.ChangeTracker
            .Entries()
            .Where(e => e.State == EntityState.Deleted && e.Entity is ISoftDeletable);

        foreach (var entry in entries)
        {
            entry.State = EntityState.Modified;
            
          
            ((ISoftDeletable)entry.Entity).Delete();
        }

        return await base.SavingChangesAsync(eventData, result, cancellationToken);
    }
}