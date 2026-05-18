using BuildingBlocks.CQRS;
using Ordering.Application.Data;

namespace Ordering.Application.Seed.Commands.ResetDb;

public class ResetDbHandler(IDatabaseService databaseService)
    : ICommandHandler<ResetDbCommand, ResetDbResult>
{
    public async Task<ResetDbResult> Handle(ResetDbCommand request, CancellationToken cancellationToken)
    {
        await databaseService.ResetDatabaseAsync(cancellationToken);

        return new ResetDbResult(true);
    }
}
