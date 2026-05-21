from beanie import PydanticObjectId
from fastapi import APIRouter, WebSocket, WebSocketDisconnect, Depends
from loguru import logger
from app.schema.message import MessageCreateRequest
from app.services.message_service import MessageService
from app.dependencies import get_message_service
from app.websocket_handlers import manager

router = APIRouter()

@router.websocket("/ws/{chat_room_id}")
async def websocket_endpoint(
    websocket: WebSocket,
    chat_room_id: PydanticObjectId,
    message_service: MessageService = Depends(get_message_service),
):
    group = f"chatroom-{chat_room_id}"
    await manager.connect(websocket, group)
    logger.info(f"WebSocket client connected to group {group}")
    try:
        while True:
            data = await websocket.receive_json()
            msg_type = data.get("type")

            if msg_type == "message":
                user_id = data.get("userId")
                username = data.get("username", "Unknown")
                content = data.get("content")

                if user_id is not None and content:
                    # Save message using service
                    req = MessageCreateRequest(
                        user_id=user_id,
                        chat_room_id=str(chat_room_id),
                        content=content.strip(),
                    )
                    saved_msg = await message_service.create_message(chat_room_id, req)

                    # Format response to client
                    broadcast_data = {
                        "type": "message",
                        "messageId": str(saved_msg.id),
                        "userId": saved_msg.user_id,
                        "username": username,
                        "content": saved_msg.content,
                        "sentAt": saved_msg.sent_at.strftime("%H:%M") if saved_msg.sent_at else "",
                    }
                    await manager.broadcast(group, broadcast_data)

            elif msg_type == "typing":
                user_id = data.get("userId")
                username = data.get("username", "Unknown")
                is_typing = data.get("isTyping", False)

                if user_id is not None:
                    broadcast_data = {
                        "type": "typing",
                        "userId": user_id,
                        "username": username,
                        "isTyping": is_typing,
                    }
                    await manager.broadcast(group, broadcast_data)

    except WebSocketDisconnect:
        manager.disconnect(websocket, group)
        logger.info(f"WebSocket client disconnected from group {group}")
    except Exception as e:
        logger.error(f"Error in websocket loop: {e}")
        manager.disconnect(websocket, group)
