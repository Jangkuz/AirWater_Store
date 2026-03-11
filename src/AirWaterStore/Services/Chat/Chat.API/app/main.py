from contextlib import asynccontextmanager
from fastapi import FastAPI
from sqlalchemy.orm import Session
from app.core.database import mongodb_startup
from app.messaging.rabbitmq import rabbitmq_startup, rabbitmq_shutdown
from app.routes import routes


@asynccontextmanager
async def lifespan(app: FastAPI):
    await mongodb_startup()
    await rabbitmq_startup()
    yield
    # await rabbitmq_shutdown()


app = FastAPI(lifespan=lifespan)
app.include_router(routes.router)


@app.get("/")
def read_root():
    return {"message": "Hello, FastAPI with Uvicorn!"}


# @app.get("/chatrooms/{user_id}", response_model=list[shemas.ChatRoomOut])
# def list_chatrooms(user_id: int, db: Session = Depends(get_db)):
#     svc = ChatRoomService(db)
#     rooms = svc.get_chatrooms_by_user(user_id)
#     return [
#         shemas.ChatRoomOut(
#             chat_room_id=r.chat_room_id, customer_id=r.customer_id, staff_id=r.staff_id
#         )
#         for r in rooms
#     ]


# @app.get("/chatrooms/details/{chat_room_id}", response_model=shemas.ChatRoomOut)
# def get_chatroom_details(chat_room_id: int, db: Session = Depends(get_db)):
#     svc = ChatRoomService(db)
#     chatroom = svc.get_chatroom_by_id(chat_room_id)
#     if not chatroom:
#         raise HTTPException(status_code=404, detail="Chatroom not found")
#     return shemas.ChatRoomOut(
#         chat_room_id=chatroom.chat_room_id,
#         customer_id=chatroom.customer_id,
#         staff_id=chatroom.staff_id,
#     )


# @app.post("/chatrooms/{customer_id}", response_model=shemas.ChatRoomOut)
# def create_or_get_chatroom(customer_id: int, db: Session = Depends(get_db)):
#     svc = ChatRoomService(db)
#     chatroom = svc.get_or_create_chatroom(customer_id)
#     return shemas.ChatRoomOut(
#         chat_room_id=chatroom.chat_room_id,
#         customer_id=chatroom.customer_id,
#         staff_id=chatroom.staff_id,
#     )


# @app.post("/chatrooms/{chat_room_id}/assign")
# def assign_staff(chat_room_id: int, staff_id: int, db: Session = Depends(get_db)):
#     svc = ChatRoomService(db)
#     svc.assign_staff_to_chatroom(chat_room_id, staff_id)
#     return {"status": "assigned"}


# @app.get("/messages/{chat_room_id}", response_model=list[shemas.MessageOut])
# def get_messages(chat_room_id: int, db: Session = Depends(get_db)):
#     msgs = db.query(models.Message).filter_by(chat_room_id=chat_room_id).all()
#     return [
#         shemas.MessageOut(
#             message_id=m.message_id,
#             chat_room_id=m.chat_room_id,
#             user_id=m.user_id,
#             content=m.content,
#             sent_at=m.sent_at,
#             username=m.user.username,
#         )
#         for m in msgs
#     ]


# @app.post("/messages", response_model=shemas.MessageOut)
# def add_message(msg: shemas.MessageCreate, db: Session = Depends(get_db)):
#     chatroom = (
#         db.query(models.ChatRoom).filter_by(chat_room_id=msg.chat_room_id).first()
#     )
#     if not chatroom:
#         raise HTTPException(status_code=404, detail="Chatroom not found")

#     message = models.Message(
#         chat_room_id=msg.chat_room_id, user_id=msg.user_id, content=msg.content.strip()
#     )
#     db.add(message)
#     db.commit()
#     db.refresh(message)

#     return shemas.MessageOut(
#         message_id=message.message_id,
#         chat_room_id=message.chat_room_id,
#         user_id=message.user_id,
#         content=message.content,
#         sent_at=message.sent_at,
#         username=message.user.username,
#     )


# @app.websocket("/ws/{chatroom_id}")
# async def websocket_endpoint(websocket: WebSocket, chatroom_id: int):
#     group = f"chatroom-{chatroom_id}"
#     await websocket_handlers.manager.connect(websocket, group)
#     try:
#         while True:
#             data = await websocket.receive_json()
#             rabbitmq.publish_broadcast({"chatroomId": chatroom_id, "message": data})
#             await websocket_handlers.manager.broadcast(group, data)
#     except Exception:
#         websocket_handlers.manager.disconnect(websocket, group)
#         await websocket.close()
