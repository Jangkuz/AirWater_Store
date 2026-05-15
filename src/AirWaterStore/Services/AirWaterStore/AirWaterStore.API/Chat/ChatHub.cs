using Microsoft.AspNetCore.SignalR;

namespace AirWaterStore.API.Chat;

public class ChatHub (
    // ISender sender,
    IHttpContextAccessor httpContextAccessor
    ) : Hub
{
    public override async Task OnConnectedAsync()
    {
        var userId = GetUserId();
        if (userId.HasValue)
        {
            // Add user to their personal group
            await Groups.AddToGroupAsync(Context.ConnectionId, $"user-{userId}");
        }

        await base.OnConnectedAsync();
    }

    private int? GetUserId()
    {
        var httpContext = httpContextAccessor.HttpContext;
        var userIdClaim = httpContext?.User.FindFirst(AppConst.UserIdClaim);
        return int.TryParse(userIdClaim?.Value, out var id) ? id : null;
    }

    private string GetUserRole()
    {
        var httpContext = httpContextAccessor.HttpContext;
        var roleClaim = httpContext?.User.FindFirst(AppConst.RoleClaim);
        var role = roleClaim?.Value;
        return role != null ? role : string.Empty;
    }

    private string GetUsername()
    {
        var httpContext = httpContextAccessor.HttpContext;
        var userNameClaim = httpContext?.User.FindFirst(AppConst.RoleClaim);
        var userName = userNameClaim?.Value;
        return userName != null ? userName : string.Empty;
    }
}
