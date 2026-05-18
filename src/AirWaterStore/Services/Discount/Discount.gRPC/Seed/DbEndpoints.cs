using Discount.gRPC.Data;
using Microsoft.EntityFrameworkCore;

namespace Discount.gRPC.Seed;

public static class DbEndpoints
{
    public static void MapDbEndpoints(this IEndpointRouteBuilder app)
    {
        app.MapDelete("/db", async (DiscountContext dbContext) =>
        {
            await dbContext.Database.EnsureDeletedAsync();
            await dbContext.Database.MigrateAsync();

            return Results.Ok(new { IsSuccess = true });
        })
        .WithName("ResetDb")
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Reset Db")
        .WithDescription("Reset Db");

        app.MapPost("/db", () =>
        {
            // Seed logic is already handled by EF Core's OnModelCreating (HasData) during MigrateAsync
            return Results.Ok(new { IsSuccess = true });
        })
        .WithName("SeedDb")
        .Produces(StatusCodes.Status200OK)
        .ProducesProblem(StatusCodes.Status400BadRequest)
        .WithSummary("Seed Db")
        .WithDescription("Seed Db");
    }
}
