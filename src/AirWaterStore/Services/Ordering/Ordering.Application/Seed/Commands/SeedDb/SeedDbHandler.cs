using BuildingBlocks.CQRS;
using Ordering.Application.Data;

namespace Ordering.Application.Seed.Commands.SeedDb;

public class SeedDbHandler(IDatabaseService databaseService)
    : ICommandHandler<SeedDbCommand, SeedDbResult>
{
    public async Task<SeedDbResult> Handle(SeedDbCommand request, CancellationToken cancellationToken)
    {
        await databaseService.SeedDatabaseAsync(cancellationToken);

        return new SeedDbResult(true);
    }
}
