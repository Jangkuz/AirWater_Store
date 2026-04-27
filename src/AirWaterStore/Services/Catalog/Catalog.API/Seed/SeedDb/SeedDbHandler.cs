using BuildingBlocks.Messaging.Events;
using MassTransit;

namespace Catalog.API.Seed.SeedDb;

public record SeedDbCommand() : ICommand<SeedDbResult>;

public record SeedDbResult(bool IsSuccess);

public class SeedDbHandler
    (IDocumentSession session,
    IPublishEndpoint publishEndpoint)
    : ICommandHandler<SeedDbCommand, SeedDbResult>
{
    public async Task<SeedDbResult> Handle(SeedDbCommand command, CancellationToken cancellationToken)
    {
        if (await session.Query<Game>().AnyAsync(cancellationToken))
        {
            return new SeedDbResult(false);
        }
        var games = await CatalogInitialData.GetPreconfigureGameAsync();

        foreach (var game in games)
        {
            session.Store<Game>(game);
            var evenMessage = new GameCreatedEvent
            {
                GameId = game.Id,
                Title = game.Title,
                Price = game.Price,
                Quantity = game.Quantity
            };


            await session.SaveChangesAsync();
            // Publish event
            await publishEndpoint.Publish(evenMessage, cancellationToken);
        }

        var reviews = await CatalogInitialData.GetPreconfigureReviewAsync();

        session.Store(reviews);

        await session.SaveChangesAsync();

        return new SeedDbResult(true);
    }
}
