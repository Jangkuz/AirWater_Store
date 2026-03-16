namespace AirWaterStore.Web.Pages.Admin.Chat;

public class IndexModel : PageModel
{
    private readonly IChatRoomService _chatRoomService;
    private readonly IAirWaterStoreService _airWaterStoreService;

    public IndexModel(
        IChatRoomService chatRoomService,
        IAirWaterStoreService airWaterStoreService)
    {
        _chatRoomService = chatRoomService;
        _airWaterStoreService = airWaterStoreService;
    }

    public List<ChatRoom> ChatRooms { get; set; } = new List<ChatRoom>();
    public Dictionary<int, string> UserNames { get; set; } = new Dictionary<int, string>();
    public string SelectedRoomId { get; set; } = "";
    // public int CurrentUserId => HttpContext.Session.GetInt32(SessionParams.UserId) ?? 0;

    public async Task<IActionResult> OnGetAsync(string selectedRoom)
    {
        // Check if user is staff
        if (!this.IsStaff())
        {
            return RedirectToPage("/Login");
        }

        SelectedRoomId = selectedRoom;

        //// Get all chat rooms (assigned to this staff or unassigned)
        var result = await _chatRoomService.GetChatRoomsByStaffId(this.GetCurrentUserId());
        ChatRooms = result.ChatRooms.ToList();

        // Load usernames
        var userIds = new HashSet<int>();
        foreach (var room in ChatRooms)
        {
            userIds.Add(room.CustomerId);
            if (room.StaffId.HasValue)
                userIds.Add(room.StaffId.Value);
        }

        foreach (var userId in userIds)
        {
            var userResult = await _airWaterStoreService.GetUserById(userId);
            UserNames[userId] = userResult?.User.UserName ?? "Unknown User";
        }

        return Page();
    }

    public string GetCustomerName(int userId)
    {
        return UserNames.TryGetValue(userId, out var name) ? name : "Unknown Customer";
    }

    public string GetStaffName(int userId)
    {
        return UserNames.TryGetValue(userId, out var name) ? name : "Unknown Staff";
    }
}