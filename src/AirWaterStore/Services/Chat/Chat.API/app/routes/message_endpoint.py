from beanie import PydanticObjectId
from fastapi import APIRouter
import starlette.status as http_status
from app.models.chatroom import ChatRoom
from app.schema.chatroom import ChatRoomResponse, ChatRoomCreateRequest


router = APIRouter()

# @router.get(
# )

# @router.post(
#     "{chat_room_id}"
# )