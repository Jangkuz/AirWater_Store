from pydantic import Field
from datetime import datetime, timezone
from beanie import Document, Indexed


class Message(Document):
    chat_room_id: str
    user_id: int
    content: str
    sent_at: datetime = Field(default_factory=lambda: datetime.now(timezone.utc))

    class Settings:
        name = "messages"
        indexes = [
            ["chat_room_id", "sent_at"],
            "sent_at",
        ]