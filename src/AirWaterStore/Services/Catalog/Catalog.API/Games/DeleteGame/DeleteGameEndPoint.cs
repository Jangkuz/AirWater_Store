namespace Catalog.API.Games.DeleteGame;

public record DeleteGameResponse(bool IsSuccess);

public class DeleteGameEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/games/{id}", async (int id, ISender sender) =>
        {
            var result = await sender.Send(new DeleteGameCommand(id));

            var response = result.Adapt<DeleteGameResponse>();

            return Results.Ok(response);
        })
        .WithName("DeleteGame")
        .Produces<DeleteGameResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .ProducesProblem(StatusCodes.Status404NotFound)
        .ProducesProblem(StatusCodes.Status500InternalServerError)
        .WithSummary("Delete Game")
        .WithDescription("Delete Game");
    }
}
