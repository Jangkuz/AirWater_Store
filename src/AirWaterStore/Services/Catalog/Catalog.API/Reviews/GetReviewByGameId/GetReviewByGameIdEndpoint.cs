namespace Catalog.API.Reviews.GetReviewByGameId;

//public record GetReviewByIdRequest();
public record GetReviewByGameIdResponse(IEnumerable<Review> Reviews);

public class GetReviewByGameIdEndpoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapGet("/games/{gameId}/reviews", async (int gameId, ISender sender) =>
        {
            var result = await sender.Send(new GetReviewByGameIdQuery(gameId));

            var response = result.Adapt<GetReviewByGameIdResponse>();

            return Results.Ok(response);
        })
        .WithName("GetReviewByGameId")
        .Produces<GetReviewByGameIdResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Get Review By Game Id")
        .WithDescription("Get Review By Game Id");
    }
}
