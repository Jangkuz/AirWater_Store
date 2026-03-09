from beanie import PydanticObjectId
from fastapi import APIRouter
import starlette.status as http_status
from app.models.chatroom import ChatRoom
from app.schema.chatroom import ChatRoomResponse, ChatRoomCreateRequest


router = APIRouter()

# @router.get(
#         "{user_id}"
# )

# @router.post(
#         "{customer_id}"
# )

@router.get(
    "/details/{chatRoomId}",
    status_code=http_status.HTTP_200_OK,
    response_description=" get chat room by id",
    name="chat_room: get_by_id",
    response_model=ChatRoomResponse,
)
async def get_chatroom_by_id(chatRoomId: PydanticObjectId):
    chatroom = await ChatRoom.get(document_id=chatRoomId)
    # chatroom = await service.get_chatroom_by_id(chatRoomId)
    response = ChatRoomResponse(
        chat_room_id=str(chatroom.id),
        customer_id=chatroom.customer_id,
        staff_id=chatroom.staff_id,
    )
    return response


@router.post(
    "{customer_id}",
    status_code=http_status.HTTP_201_CREATED,
    response_description="create chat room",
    name="chat_room: create",
    response_model=ChatRoomResponse,
)
async def create(
    request: ChatRoomCreateRequest,
):
    # chatroom = await service.get_or_create_chatroom(request.customer_id)
    chatroom = await ChatRoom.insert_one(
        ChatRoom(
            customer_id=request.customer_id,
        )
    )
    response = ChatRoomResponse(
        chat_room_id=str(chatroom.id),
        customer_id=chatroom.customer_id,
        staff_id=chatroom.staff_id,
    )
    return response

# @router.post(
#     "{chat_room_id}/assign"
# )