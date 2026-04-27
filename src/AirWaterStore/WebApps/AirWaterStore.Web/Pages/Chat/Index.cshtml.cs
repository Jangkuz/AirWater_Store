using AirWaterStore.Web.Models.Chat;

namespace AirWaterStore.Web.Pages.Chat
{
    public class IndexModel : PageModel
    {
        private readonly IChatRoomService _chatRoomService;
        private readonly IAirWaterStoreService _airWaterStoreService;

        public IndexModel(
            IChatRoomService chatRoomService,
            IAirWaterStoreService airWaterStorerService
        )
        {
            _chatRoomService = chatRoomService;
            _airWaterStoreService = airWaterStorerService;
        }

        public ChatRoom ChatRoom { get; set; } = default!;
        public List<Message> Messages { get; set; } = new List<Message>();
        public Dictionary<int, string> UserNames { get; set; } = new Dictionary<int, string>();

        public async Task<IActionResult> OnGetAsync()
        {
            var userId = this.GetCurrentUserId();

            if (!this.IsAuthenticated())
            {
                return RedirectToPage(AppRouting.Login);
            }

            // Only customers can access this page
            if (!this.IsCustomer())
            {
                return RedirectToPage(AppRouting.AdminChat);
            }

            var createChatroomReq = new CreateChatRoomRequest(userId);

            // Get or create chat room for customer
            var result = await _chatRoomService.GetOrCreateChatRoom(createChatroomReq);

            ChatRoom = result.ChatRoom;

            // Get messages
            var messageResult = await _chatRoomService.GetMessagesByChatRoomId(ChatRoom.ChatRoomId);
            Messages = messageResult.Messages.ToList();

            // Load usernames
            var userIds = Messages.Select(m => m.UserId).Distinct().ToList();
            userIds.Add(userId);

            foreach (var id in userIds)
            {
                var user = await _airWaterStoreService.GetUserById(id);
                UserNames[id] = user?.User.UserName ?? "Unknown User";
            }

            return Page();
        }

        public async Task<IActionResult> OnPostSendMessageAsync(string messageContent)
        {

            var userId = this.GetCurrentUserId();
            if (userId != 0 || string.IsNullOrWhiteSpace(messageContent))
            {
                return RedirectToPage();
            }

            var createChatRoomReq = new CreateChatRoomRequest(userId);

            var result = await _chatRoomService.GetOrCreateChatRoom(createChatRoomReq);

            var message = new CreateMessageRequest
            (
                ChatRoomId: result.ChatRoom.ChatRoomId,
                UserId: userId,
                Content: messageContent.Trim(),
                SentAt: DateTime.Now
            );

            await _chatRoomService.PostMessage(message);

            return RedirectToPage();
        }

        public string GetUsername(int userId)
        {
            return UserNames.TryGetValue(userId, out var username) ? username : "Unknown User";
        }
    }
}