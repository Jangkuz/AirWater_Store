using BuildingBlocks.CQRS;
using MassTransit;
using Microsoft.AspNetCore.Identity;
using AirWaterStore.API.Extentions;

namespace AirWaterStore.API.Seed.SeedDb;

public record SeedDbCommand() : ICommand<SeedDbResult>;

public record SeedDbResult(bool IsSuccess);

public class SeedDbHandler
    (ApplicationDbContext context,
     UserManager<User> userManager,
     RoleManager<Role> roleManager,
     IPublishEndpoint publishEndpoint)
    : ICommandHandler<SeedDbCommand, SeedDbResult>
{
    public async Task<SeedDbResult> Handle(SeedDbCommand request, CancellationToken cancellationToken)
    {
        await DatabaseExtentions.SeedRole(roleManager);
        await DatabaseExtentions.SeedUser(userManager, context, publishEndpoint);

        return new SeedDbResult(true);
    }
}
