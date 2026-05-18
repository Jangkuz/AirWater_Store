using Microsoft.EntityFrameworkCore;
using Ordering.Application.Data;
using Ordering.Infrastructure.Data.Extentions;

namespace Ordering.Infrastructure.Data;

public class DatabaseService(ApplicationDbContext context) : IDatabaseService
{
    public async Task ResetDatabaseAsync(CancellationToken cancellationToken = default)
    {
        await context.Database.EnsureDeletedAsync(cancellationToken);
        await context.Database.MigrateAsync(cancellationToken);
    }

    public async Task SeedDatabaseAsync(CancellationToken cancellationToken = default)
    {
        await DatabaseExtentions.SeedAsync(context);
    }
}
