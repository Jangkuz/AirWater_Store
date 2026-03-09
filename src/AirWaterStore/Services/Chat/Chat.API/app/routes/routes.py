from fastapi import APIRouter
from app.routes import chatroom_endpoint

router = APIRouter()

router.include_router(chatroom_endpoint.router, prefix="/chatrooms", tags=["Chatrooms"])
# router.include_router(message_endpoint.router, prefix="/messages", tags=["Messages"])
