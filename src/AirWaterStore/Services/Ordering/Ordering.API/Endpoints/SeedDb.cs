using Carter;
using Mapster;
using MediatR;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Routing;
using Ordering.Application.Seed.Commands.SeedDb;

namespace Ordering.API.Endpoints;

public record SeedDbResponse(bool IsSuccess);

public class SeedDb : ICarterModule
{
    public void AddRoutes(IEndpointRouteBuilder app)
    {
        app.MapPost("/db", async (ISender sender) =>
        {
            var result = await sender.Send(new SeedDbCommand());
            var response = result.Adapt<SeedDbResponse>();
            return Results.Ok(response);
        })
        .WithName("SeedDb")
        .Produces<SeedDbResponse>(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Seed Db")
        .WithDescription("Seed Db");
    }
}
