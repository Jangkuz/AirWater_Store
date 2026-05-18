using BuildingBlocks.CQRS;
using Microsoft.EntityFrameworkCore;
using AirWaterStore.API.Data; // Assuming this is where ApplicationDbContext is, wait let me check namespace. I'll just use global namespace or AirWaterStore.API
using AirWaterStore.API.Models; // For User/Role if needed

namespace AirWaterStore.API.Seed.ResetDb;

public record ResetDbCommand() : ICommand<ResetDbResult>;

public record ResetDbResult(bool IsSuccess);

public class ResetDbHandler
    (ApplicationDbContext context)
    : ICommandHandler<ResetDbCommand, ResetDbResult>
{
    public async Task<ResetDbResult> Handle(ResetDbCommand request, CancellationToken cancellationToken)
    {
        await context.Database.EnsureDeletedAsync(cancellationToken);
        await context.Database.MigrateAsync(cancellationToken);

        return new ResetDbResult(true);
    }
}
