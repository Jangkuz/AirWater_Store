from bson import ObjectId
from typing import List, Optional
from beanie.operators import Or
from loguru import logger
from app.core.config import settings
from app.models.chatroom import ChatRoom
from pymongo.errors import DuplicateKeyError


class ChatRoomService:

    async def get_or_create_chatroom(self, customer_id: int) -> ChatRoom:
        existing = await ChatRoom.find_one(ChatRoom.customer_id == customer_id)

        if existing:
            return existing

        chatRoom = ChatRoom(customer_id=customer_id, staff_id=None)

        try:
            return await chatRoom.insert()

        except DuplicateKeyError as e:
            logger.error(e.details)
            return await ChatRoom.find_one(ChatRoom.customer_id == customer_id)

    async def get_chatroom_by_id(self, chat_room_id: str) -> Optional[ChatRoom]:
        return await ChatRoom.get(chat_room_id)

    async def get_chatrooms_by_user(self, user_id: int) -> List[ChatRoom]:
        return await ChatRoom.find(Or(ChatRoom.customer_id == user_id,
                                       ChatRoom.staff_id == user_id)).to_list()

    async def assign_staff_to_chatroom(self, chat_room_id: str, staff_id: int) -> Optional[ChatRoom]:
        chatroom = await self.get_chatroom_by_id(chat_room_id)
        if chatroom:
            chatroom.staff_id = staff_id
            await chatroom.save()
        return chatroom


