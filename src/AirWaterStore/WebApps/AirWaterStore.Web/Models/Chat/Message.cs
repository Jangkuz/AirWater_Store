namespace AirWaterStore.Web.Models.Chat;

public partial class Message
{
    public int MessageId { get; set; }

    public string ChatRoomId { get; set; } = "";

    public int UserId { get; set; }

    public string Content { get; set; } = null!;

    public DateTime? SentAt { get; set; }
}

public record GetMessagesByChatRoomIdResponse(IEnumerable<Message> Messages);

public record CreateMessageRequest(
    string ChatRoomId,
    int UserId,
    string Content,
    DateTime SentAt
    );

public record CreateMessageResponse(string Id);
