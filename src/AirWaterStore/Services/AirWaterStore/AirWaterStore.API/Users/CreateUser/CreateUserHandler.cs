using BuildingBlocks.Messaging.Events;
using MassTransit;

namespace AirWaterStore.API.Users.CreateUser;

public record CreateUserCommand(
    string UserName,
    string Email,
    string Password
    ) : ICommand<CreateUserResult>;

public record CreateUserResult(int Id);

//TODO: add validator

internal class CreateUserHandler(
    UserManager<User> userManager,
    IPublishEndpoint publishEndpoint
    ) : ICommandHandler<CreateUserCommand, CreateUserResult>
{
    public async Task<CreateUserResult> Handle(CreateUserCommand command, CancellationToken cancellationToken)
    {
        var user = new User
        {
            UserName = command.UserName,
            Email = command.Email,
            //Password = command.Password,
            IsBan = false
        };

        var result = await userManager.CreateAsync(user, command.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join("; ", result.Errors.Select(e => e.Description));
            throw new BadRequestException(errors);
        }

        await userManager.AddToRoleAsync(user, AppConst.User);
        IList<String> roles = await userManager.GetRolesAsync(user);

        var integrationEvent = new UserCreatedEvent
        {
            UserId = user.Id,
            UserName = user.UserName,
            Email = user.Email,
            Roles = roles
        };

        await publishEndpoint.Publish(integrationEvent, cancellationToken);

        //TODO: Return login token for auto login

        return new CreateUserResult(user.Id);
    }
}
