namespace AirWaterStore.Web.Pages.Admin.Chat;

public class RoomModel : PageModel
{
    private readonly IChatRoomService _chatRoomService;
    private readonly IAirWaterStoreService _airWaterStoreService;

    public RoomModel(
        IChatRoomService chatRoomService,
        IAirWaterStoreService airWaterStoreService
        )
    {
        _chatRoomService = chatRoomService;
        _airWaterStoreService = airWaterStoreService;
    }

    public ChatRoom ChatRoom { get; set; } = default!;
    public List<Message> Messages { get; set; } = new List<Message>();
    public string CustomerName { get; set; } = string.Empty;
    public Dictionary<int, string> UserNames { get; set; } = new Dictionary<int, string>();
    // public int CurrentUserId => HttpContext.Session.GetInt32(SessionParams.UserId) ?? 0;

    public async Task<IActionResult> OnGetAsync(string id)
    {
        // Check if user is staff
        if (!this.IsStaff())
        {
            return RedirectToPage("/Login");
        }

        var chatRoomResult = await _chatRoomService.GetChatRoomById(id);
        if (chatRoomResult == null)
        {
            return NotFound();
        }

        ChatRoom = chatRoomResult.ChatRoom;

        // Get messages
        var messageResult = await _chatRoomService.GetMessagesByChatRoomId(id);
        Messages = messageResult.Messages.ToList();

        // Load usernames
        var userIds = Messages.Select(m => m.UserId).Distinct().ToList();
        userIds.Add(ChatRoom.CustomerId);
        if (ChatRoom.StaffId.HasValue)
            userIds.Add(ChatRoom.StaffId.Value);

        foreach (var userId in userIds)
        {
            var userResult = await _airWaterStoreService.GetUserById(userId);
            UserNames[userId] = userResult?.User.UserName ?? "Unknown User";
        }

        CustomerName = GetUsername(ChatRoom.CustomerId);

        return Page();
    }

    public async Task<IActionResult> OnPostSendMessageAsync(string chatRoomId, string messageContent)
    {
        if (!this.IsStaff() || string.IsNullOrWhiteSpace(messageContent))
        {
            return RedirectToPage();
        }

        var chatRoomResult = await _chatRoomService.GetChatRoomById(chatRoomId);
        var chatRoom = chatRoomResult.ChatRoom;
        if (chatRoom == null)
        {
            return NotFound();
        }

        // If chat is unassigned, assign it to current staff
        if (!chatRoom.StaffId.HasValue)
        {
            await _chatRoomService.AssignStaffToChatRoom(new AssignStaffToChatRoomRequest(
                ChatRoomId: chatRoomId,
                StaffId: this.GetCurrentUserId()
                ));
        }

        // Send message
        var message = new CreateMessageRequest
        (
            ChatRoomId: chatRoomId,
            UserId: this.GetCurrentUserId(),
            Content: messageContent.Trim(),
            SentAt: DateTime.Now
        );

        await _chatRoomService.PostMessage(message);

        return RedirectToPage(new { id = chatRoomId });
    }

    public string GetUsername(int userId)
    {
        return UserNames.TryGetValue(userId, out var username) ? username : "Unknown User";
    }
}