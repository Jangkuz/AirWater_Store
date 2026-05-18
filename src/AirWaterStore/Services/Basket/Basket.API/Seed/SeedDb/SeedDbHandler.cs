using BuildingBlocks.CQRS;

namespace Basket.API.Seed.SeedDb;

public record SeedDbCommand() : ICommand<SeedDbResult>;

public record SeedDbResult(bool IsSuccess);

public class SeedDbHandler
    : ICommandHandler<SeedDbCommand, SeedDbResult>
{
    public Task<SeedDbResult> Handle(SeedDbCommand request, CancellationToken cancellationToken)
    {
        // No initial data to seed for Basket
        return Task.FromResult(new SeedDbResult(true));
    }
}
