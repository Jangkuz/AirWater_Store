from fastapi import APIRouter, HTTPException, status
from loguru import logger

from app.core.config import settings
from app.models.chatroom import ChatRoom

router = APIRouter()


@router.delete("/db", status_code=status.HTTP_200_OK, name="database: reset")
async def reset_database():
    """Clears all development data from the database."""
    if not settings.DEBUG:
        raise HTTPException(
            status_code=status.HTTP_403_FORBIDDEN,
            detail="Database reset is only allowed in DEBUG mode.",
        )

    logger.info("Resetting database (ChatRooms)...")
    await ChatRoom.delete_all()
    # NOTE: Add await Message.delete_all() here once your message model is set up

    return {"message": "Database reset successfully."}


@router.post("/db", status_code=status.HTTP_201_CREATED, name="database: seed")
async def seed_database():
    """Seeds the database with initial development data."""
    if not settings.DEBUG:
        raise HTTPException(
            status_code=status.HTTP_403_FORBIDDEN,
            detail="Database seeding is only allowed in DEBUG mode.",
        )

    # logger.info("Seeding database (ChatRooms)...")
    # sample_rooms = [
    #     ChatRoom(customer_id=1, staff_id=101),
    #     ChatRoom(customer_id=2, staff_id=None),
    #     ChatRoom(customer_id=3, staff_id=102),
    # ]
    # await ChatRoom.insert_many(sample_rooms)

    return {
        "message": "Database seeded successfully.",
        # "seeded_count": len(sample_rooms),
    }
