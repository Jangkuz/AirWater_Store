using BuildingBlocks.Data;
using MassTransit;

namespace Catalog.API.Data;

public class CatalogInitialData  : IInitialData
{
    public async Task Populate(IDocumentStore store, CancellationToken cancellation)
    {
        using var session = store.LightweightSession();
        //if (!await session.Query<Game>().AnyAsync())
        //{
        //    var games = await GetPreconfigureGameAsync();

        //    foreach (var game in games)
        //    {
        //    session.Store<Game>(await GetPreconfigureGameAsync());
        //        var evenMessage = new GameCreatedEvent
        //        {
        //            GameId = game.Id,
        //            Title = game.Title,
        //            Price = game.Price
        //        };

        //        await publishEndpoint.Publish(evenMessage, cancellation);
        //    }

        //    await session.SaveChangesAsync();
        //}

        //if (!await session.Query<Review>().AnyAsync())
        //{
        //    session.Store<Review>(await GetPreconfigureReviewAsync());
        //    await session.SaveChangesAsync();
        //}
    }

    public static async Task<IEnumerable<Game>> GetPreconfigureGameAsync()
    {

        var rawList = await ReadGameData.ReadMockDataAsync();

        return rawList
            .Select(ConvertToGame)
            .ToList();
    }

    public static async Task<IEnumerable<Review>> GetPreconfigureReviewAsync()
    {
        var reviewJsonList = await ReadReviewData.ReadMockDataAsync();

        var userJsonList = await ReadUserData.ReadMockDataAsync();

        var reviews = reviewJsonList
            .Select(ConvertToReview)
            .ToList();

        return MapUserName(reviews, userJsonList);
    }

    private static Game ConvertToGame(GameJsonDto dto)
    {
        //IMPORTANT: this exist because the current mock data file is a string<Python List> so it needed to be process.
        List<string> genreList = ReadGameData.ParseTags(dto.Tags);

        return new Game
        {
            Id = dto.AppId,
            Title = dto.Title,
            ThumbnailUrl = dto.ThumbnailUrl,
            Description = dto.Description,
            Genres = genreList ?? new List<string>(),
            Developer = dto.Developer,
            Publisher = dto.Publisher,
            ReleaseDate = ReadGameData.ParseDateOnly(dto.ReleaseDate),
            Price = ReadGameData.ParsePrice(dto.Price),
            Quantity = Random.Shared.Next(50, 500) // mock quantity
        };
    }

    private static Review ConvertToReview(ReviewJsonDto dto)
    {
        return new Review
        {
            Id = dto.ReviewId,
            UserId = dto.UserId,
            GameId = dto.GameId,
            Rating = dto.Rating,
            Comment = dto.Comment,
            ReviewDate = dto.ReviewDate
        };
    }

    private static IEnumerable<Review> MapUserName(IEnumerable<Review> reviews, IEnumerable<UserJsonDto> users)
    {
        var userDictionary = users.ToDictionary(u => u.UserId, u => u.UserName);

        foreach (var review in reviews)
        {
            if (userDictionary.TryGetValue(review.Id, out var userName))
            {
                review.UserName = userName;
            }
            else
            {
                review.UserName = "Unknown";
            }
        }

        return reviews;
    }
}
