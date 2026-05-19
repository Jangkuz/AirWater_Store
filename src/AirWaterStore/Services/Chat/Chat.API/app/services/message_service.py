from typing import List
from beanie import PydanticObjectId
from app.models.message import Message
from app.schema.message import MessageCreateRequest
from loguru import logger


class MessageService:

    async def get_messages_by_chatroom(
        self, chat_room_id: PydanticObjectId
    ) -> List[Message]:
        messages = await Message.find(
            Message.chat_room_id == str(chat_room_id)
        ).to_list()
        # return sorted(messages, key=lambda m: m.send_at)
        return messages

    async def create_message(
        self, chat_room_id: PydanticObjectId, request: MessageCreateRequest
    ) -> Message:
        message = Message(
            chat_room_id=str(chat_room_id),
            user_id=request.user_id,
            content=request.content,
        )

        try:
            return await message.insert()
        except Exception as e:
            # logger.exception automatically includes the full stack trace in your terminal
            logger.exception("Failed to insert message into the database")
            # Re-raise so the router or global exception handler can catch it and return a 500 status
            raise RuntimeError("Database error occurred while creating message.") from e
