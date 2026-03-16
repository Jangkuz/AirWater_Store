namespace AirWaterStore.Web.Models.Chat;

public partial class ChatRoom
{
    public string ChatRoomId { get; set; } = "";

    public int CustomerId { get; set; }

    public int? StaffId { get; set; }

    //public virtual User Customer { get; set; } = null!;

    //public virtual ICollection<Message> Messages { get; set; } = new List<Message>();

    //public virtual User? Staff { get; set; }
}
public record GetChatRoomsResponse(ChatRoom ChatRoom);
public record GetStaffChatRoomsResponse(IEnumerable<ChatRoom> ChatRooms);

public record AssignStaffToChatRoomRequest(
    string ChatRoomId,
    int StaffId
    );

public record CreateChatRoomRequest(
    int CustomerId
    );
