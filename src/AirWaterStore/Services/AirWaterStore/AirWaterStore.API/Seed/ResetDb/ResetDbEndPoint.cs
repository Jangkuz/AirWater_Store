using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;

namespace AirWaterStore.API.Seed.ResetDb;

public record ResetDbResponse(bool IsSuccess);

public class ResetDbEndPoint : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapDelete("/db", async (ISender sender) =>
        {
            var result = await sender.Send(new ResetDbCommand());

            var response = result.Adapt<ResetDbResponse>();

            return Results.Ok(response);
        })
        .WithName("ResetDb")
        .Produces<ResetDbResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Reset Db")
        .WithDescription("Reset Db");
    }
}
