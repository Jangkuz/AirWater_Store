using System.Security.Claims;

namespace AirWaterStore.Web.Extentions;

public static class PageModelExtentions
{
    public static int GetCurrentUserId(this PageModel page)
    {
        return page.User.GetUserId();
    }
    public static string GetCurrentUserName(this PageModel page)
    {
        return page.User.GetUserName();
    }
    public static bool IsAuthenticated(this PageModel page)
    {
        return page.User.GetUserId() != 0;
    }
    public static bool IsCustomer(this PageModel page)
    {
        return page.User.IsCustomer();
    }
    public static bool IsStaff(this PageModel page)
    {
        return page.User.IsStaff();
    }

    public static int GetUserId(this ClaimsPrincipal user)
        => int.TryParse(user.FindFirst(AppConst.UserIdClaim)?.Value, out var id) ? id : 0;

    public static string GetUserName(this ClaimsPrincipal user)
        => user.FindFirst(AppConst.UserNameClaim)?.Value ?? "Unknown User";

    public static string GetRole(this ClaimsPrincipal user)
    {
        var userRole = user.FindFirst(AppConst.RoleClaim)?.Value ?? "";
        return userRole;
    }
    public static bool IsCustomer(this ClaimsPrincipal user)
        => user.GetRole() == AppConst.User;

    public static bool IsStaff(this ClaimsPrincipal user)
        => user.GetRole() == AppConst.Staff;

    public static bool IsBan(this ClaimsPrincipal user)
        => bool.TryParse(user.FindFirst(AppConst.IsBanClaim)?.Value, out bool isBan) ? isBan : true;


}
