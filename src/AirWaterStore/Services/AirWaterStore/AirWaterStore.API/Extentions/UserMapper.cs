namespace AirWaterStore.API.Extentions;

public class UserMapper(UserManager<User> userManager)
{
    public IEnumerable<UserDto> ToUserDtosList(IEnumerable<User> users)
    {
        return users.Select(u => DtoFromUser(u));
    }

    public UserDto ToUserDto(User user)
    {
        return DtoFromUser(user);
    }

    private UserDto DtoFromUser(User user)
    {
        return new UserDto(
            Id: user.Id,
            UserName: user.UserName ?? string.Empty,
            Email: user.Email ?? string.Empty,
            IsBan: user.IsBan,
            Roles: userManager.GetRolesAsync(user).GetAwaiter().GetResult()
            );
    }
}
