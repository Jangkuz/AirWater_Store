using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Ordering.Infrastructure.Data.Extentions;
public static class DatabaseExtentions
{
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var logger = scope.ServiceProvider.GetRequiredService<ILogger<WebApplication>>();

        //await context.Database.MigrateAsync();
        //context.Database.MigrateAsync().GetAwaiter().GetResult();

        var timeout = TimeSpan.FromMinutes(1);
        var start = DateTime.UtcNow;
        var delay = TimeSpan.FromSeconds(5);

        while (true)
        {
            try
            {
                await context.Database.MigrateAsync();
                logger.LogInformation("Database migration successful.");
                break;
            }
            catch (Exception ex)
            {
                if (DateTime.UtcNow - start > timeout)
                {
                    logger.LogError(ex, "Database migration failed after multiple attempts.");
                    throw;
                }

                logger.LogWarning(ex, "Database migration failed. Retrying in {Delay}...", delay);
                await Task.Delay(delay);
            }
        }

        //await SeedAsync(context);
    }

    public static async Task SeedAsync(ApplicationDbContext context)
    {
        await SeedCustomerAsync(context);
        //await SeedGameAsync(context);
        await SeedOrdersWithItemsAsync(context);
    }

    private static async Task SeedCustomerAsync(ApplicationDbContext context)
    {
        if (!await context.Customers.AnyAsync())
        {
            var customers = await InitialData.CustomersAsync();
            await context.Customers.AddRangeAsync(customers);
            await context.SaveChangesAsync();
        }
    }

    //private static async Task SeedGameAsync(ApplicationDbContext context)
    //{
    //    if (!await context.Games.AnyAsync())
    //    {
    //        var games = await InitialData.GamesAsync();
    //        await context.Games.AddRangeAsync(games);
    //        await context.SaveChangesAsync();
    //    }
    //}

    private static async Task SeedOrdersWithItemsAsync(ApplicationDbContext context)
    {
        if (!await context.Orders.AnyAsync())
        {
            //await context.Orders.AddRangeAsync(InitialData.OrdersWithItems);
            //await context.SaveChangesAsync();
        }
    }
}

