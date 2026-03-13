from typing import List

from beanie import PydanticObjectId
from fastapi import APIRouter, Depends, HTTPException
import starlette.status as http_status
from app.models.chatroom import ChatRoom
from app.schema.chatroom import ChatRoomResponse, ChatRoomCreateRequest, ChatRoomUpdateStaffRequest
from app.dependencies import get_chatroom_service
from app.services.chatroom_service import ChatRoomService


router = APIRouter()

# @router.get(
#         "{user_id}"
# )
@router.get(
    "/{userId}",
    status_code=http_status.HTTP_200_OK,
    response_description=" get user's chat rooms",
    name="chat_room: get_by_user",
    response_model=List[ChatRoomResponse],
)
async def get_chatrooms_by_user(
    userId: int,
    service: ChatRoomService = Depends(get_chatroom_service)
):
    chatRooms = await service.get_chatrooms_by_user(userId)
    response = []
    for room in chatRooms:
        response.append(
            ChatRoomResponse(
                chat_room_id=str(room.id),
                customer_id=room.customer_id,
                staff_id=room.staff_id,
            )
        )

    return response


@router.get(
    "/{chatRoomId}/details",
    status_code=http_status.HTTP_200_OK,
    response_description=" get chat room by id",
    name="chat_room: get_by_id",
    response_model=ChatRoomResponse,
)
async def get_chatroom_by_id(
    chatRoomId: PydanticObjectId,
    service: ChatRoomService = Depends(get_chatroom_service),
):
    chatroom = await service.get_chatroom_by_id(chatRoomId)

    if not chatroom:
        raise HTTPException(
            status_code=http_status.HTTP_404_NOT_FOUND,
            detail=f"Chatroom with ID {chatRoomId} not found",
        )

    response = ChatRoomResponse(
        chat_room_id=str(chatroom.id),
        customer_id=chatroom.customer_id,
        staff_id=chatroom.staff_id,
    )
    return response


@router.post(
    "/{customer_id}",
    status_code=http_status.HTTP_201_CREATED,
    response_description="create chat room",
    name="chat_room: create",
    response_model=ChatRoomResponse,
)
async def create(
    customer_id: int,
    # request: ChatRoomCreateRequest,
    service: ChatRoomService = Depends(get_chatroom_service),
):
    chatroom = await service.get_or_create_chatroom(customer_id)
    response = ChatRoomResponse(
        chat_room_id=str(chatroom.id),
        customer_id=chatroom.customer_id,
        staff_id=chatroom.staff_id,
    )
    return response


@router.post(
    "/{chat_room_id}/assign",
    status_code=http_status.HTTP_200_OK,
    response_description="assign staff to chat room",
    name="chat_room: assign_staff",
    response_model=ChatRoomResponse,
)
async def assign_staff(
    chat_room_id: PydanticObjectId,
    request: ChatRoomUpdateStaffRequest,
    service: ChatRoomService = Depends(get_chatroom_service)
):
    chatroom = await service.get_chatroom_by_id(chat_room_id)

    if not chatroom:
        raise HTTPException(
            status_code=http_status.HTTP_404_NOT_FOUND,
            detail=f"Chatroom with ID {chat_room_id} not found",
        )

    chatroom = await service.assign_staff_to_chatroom(chat_room_id, request.staff_id)

    return ChatRoomResponse(
        chat_room_id=str(chatroom.id),
        customer_id=chatroom.customer_id,
        staff_id=chatroom.staff_id,
    )
