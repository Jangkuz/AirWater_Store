from app.services.chatroom_service import ChatRoomService
from app.services.message_service import MessageService

def get_chatroom_service() -> ChatRoomService:
    return ChatRoomService()

def get_message_service() -> MessageService:
    return MessageService()