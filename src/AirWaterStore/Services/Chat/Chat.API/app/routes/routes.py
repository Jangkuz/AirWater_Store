from fastapi import APIRouter
from app.routes import chatroom_endpoint, db_endpoint, message_endpoint

router = APIRouter()

router.include_router(chatroom_endpoint.router, prefix="/chatrooms", tags=["Chatrooms"])
router.include_router(db_endpoint.router, tags=["Database"])
router.include_router(message_endpoint.router, prefix="/chatrooms", tags=["Messages"])
