namespace Catalog.API.Seed.SeedDb;

public record SeedDbResponse(bool IsSuccess);

public class SeedDbEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/db", async (ISender sender) =>
        {
            var result = await sender.Send(new SeedDbCommand());

            var response = result.Adapt<SeedDbResult>();

            return Results.Ok(response);
        })
        .WithName("SeedDb")
        .Produces<SeedDbResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Seed Db")
        .WithDescription("Seed Db");
    }
}
