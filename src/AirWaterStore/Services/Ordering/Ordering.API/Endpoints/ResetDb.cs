using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Ordering.Application.Seed.Commands.ResetDb;

namespace Ordering.API.Endpoints;

public record ResetDbResponse(bool IsSuccess);

public class ResetDb : ICarterModule
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
