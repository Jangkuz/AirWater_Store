namespace Ordering.Application.Data;

public interface IDatabaseService
{
    Task ResetDatabaseAsync(CancellationToken cancellationToken = default);
    Task SeedDatabaseAsync(CancellationToken cancellationToken = default);
}
