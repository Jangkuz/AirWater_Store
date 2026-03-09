using AirWaterStore.API.Users.CreateUser;
using BuildingBlocks.Data;
using BuildingBlocks.Messaging.Events;
using MassTransit;
using Microsoft.EntityFrameworkCore;

namespace AirWaterStore.API.Extentions;

public static class DatabaseExtentions
{
    private const string PASSWORD = "123456As!";
    public static async Task InitialiseDatabaseAsync(this WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        var userManager = scope.ServiceProvider.GetRequiredService<UserManager<User>>();
        var publishEndpoint = scope.ServiceProvider.GetRequiredService<IPublishEndpoint>();
        var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<Role>>();

        await context.Database.MigrateAsync();

        await SeedRole(roleManager);

        await SeedUser(
            userManager,
            context,
            publishEndpoint
            );
    }

    private static async Task SeedRole(RoleManager<Role> roleManager)
    {
        string[] roleNames = { AppConst.Admin,
        AppConst.Staff,
        AppConst.User};
        foreach (var role in roleNames)
        {
            if (!await roleManager.RoleExistsAsync(role))
                await roleManager.CreateAsync(new Role(role));
        }
    }

    private static async Task SeedUser(
        UserManager<User> userManager,
        ApplicationDbContext context,
        IPublishEndpoint publishEndpoint
        )
    {
        if (await context.Users.AnyAsync())
        {
            return;
        }

        await SeedTestAccount(userManager, publishEndpoint);

        var rawList = await ReadUserData.ReadMockDataAsync();
        var users = rawList.Select(ConvertToUser).ToList();

        foreach (var user in users)
        {
            await userManager.CreateAsync(user, PASSWORD);
            await userManager.AddToRoleAsync(user, AppConst.User);
            IList<String> roles = await userManager.GetRolesAsync(user);
            var integrationEvent = new UserCreatedEvent
            {
                UserId = user.Id,
                UserName = user.UserName!,
                Email = user.Email!,
                Roles = roles
            };
            await publishEndpoint.Publish(integrationEvent);
        }
    }

    private static User ConvertToUser(UserJsonDto dto)
    {
        return new User
        {
            Id = dto.UserId,
            UserName = dto.UserName,
            Email = dto.Email,
            IsBan = false,
        };
    }

    private static async Task SeedTestAccount(
        UserManager<User> userManager,
        IPublishEndpoint publishEndpoint
        )
    {
        var user = new User
        {
            UserName = "User",
            Email = "user@gmail.com",
            IsBan = false
        };

        await userManager.CreateAsync(user, PASSWORD);
        await userManager.AddToRoleAsync(user, AppConst.User);
        var userEvent = new UserCreatedEvent
        {
            UserId = user.Id,
            UserName = user.UserName,
            Email = user.Email
        };

        var staff = new User
        {
            UserName = "Staff",
            Email = "staff@gmail.com",
            IsBan = false
        };
        await userManager.CreateAsync(staff, PASSWORD);
        await userManager.AddToRoleAsync(staff, AppConst.Staff);
        var staffEvent = new UserCreatedEvent
        {
            UserId = staff.Id,
            UserName = staff.UserName,
            Email = staff.Email
        };
        //publishEndpoint.Publish(userEvent).GetAwaiter().GetResult();
        publishEndpoint.Publish(userEvent).GetAwaiter().GetResult();
        publishEndpoint.Publish(staffEvent).GetAwaiter().GetResult();
    }
}
