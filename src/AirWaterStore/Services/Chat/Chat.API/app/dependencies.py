from app.services.chatroom_service import ChatRoomService

def get_chatroom_service() -> ChatRoomService:
    return ChatRoomService()