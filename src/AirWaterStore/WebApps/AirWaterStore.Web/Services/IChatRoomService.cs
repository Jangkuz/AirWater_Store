namespace AirWaterStore.Web.Services;

public interface IChatRoomService
{
    [Get("/chatroom-service/chatrooms/{chatRoomId}")]
    Task<GetChatRoomsResponse> GetChatRoomById(string chatRoomId);
    [Get("/chatroom-service/chatrooms/user/{userId}")]
    Task<GetStaffChatRoomsResponse> GetChatRoomsByStaffId(int userId);
    [Post("/chatroom-service/chatrooms")]
    Task<GetChatRoomsResponse> GetOrCreateChatRoom();
    [Post("/chatroom-service/chatrooms/{chatRoomId}/assign")]
    Task<GetChatRoomsResponse> AssignStaffToChatRoom(AssignStaffToChatRoomRequest request);

    //=======================================
    [Get("/chatroom-service/chatrooms/{chatRoomId}/messages")]
    Task<GetMessagesByChatRoomIdResponse> GetMessagesByChatRoomId(string chatRoomId);
    [Post("/chatroom-service/messages")]
    Task<GetMessagesByChatRoomIdResponse> PostMessage(CreateMessageRequest messageDto);
}
