from typing import List
from beanie import PydanticObjectId
from fastapi import APIRouter, Depends, HTTPException
import starlette.status as http_status
from app.models.message import Message
from app.schema.message import (
    MessageResponse,
    MessageCreateRequest,
    MessageResponseEnvelope,
    MessageListResponseEnvelope,
)
from app.services.message_service import MessageService
from app.dependencies import get_message_service

router = APIRouter()


@router.get(
    "/{chatRoomId}/messages",
    status_code=http_status.HTTP_200_OK,
    response_description="get messages by chat room",
    name="message: get by chat room",
    response_model=MessageListResponseEnvelope,
)
async def get_messages_by_chatroom(
    chatRoomId: PydanticObjectId,
    service: MessageService = Depends(get_message_service),
):
    messages = await service.get_messages_by_chatroom(chatRoomId)

    response = []
    for m in messages:
        response.append(
            MessageResponse(
                message_id=str(m.id),
                chat_room_id=m.chat_room_id,
                user_id=m.user_id,
                content=m.content,
                sent_at=m.sent_at,
            )
        )
    return MessageListResponseEnvelope(messages=response)


# TODOs: User wrapper class for list
@router.post(
    "/{chatRoomId}/messages",
    status_code=http_status.HTTP_201_CREATED,
    response_description="create a new message",
    name="message: create",
    response_model=MessageResponseEnvelope,
)
async def create_message(
    chatRoomId: PydanticObjectId,
    request: MessageCreateRequest,
    service: MessageService = Depends(get_message_service),
):
    message = await service.create_message(chatRoomId, request)

    response = MessageResponse(
        message_id=str(message.id),
        chat_room_id=message.chat_room_id,
        user_id=message.user_id,
        content=message.content,
        sent_at=message.sent_at,
    )
    return MessageResponseEnvelope(message=response)
