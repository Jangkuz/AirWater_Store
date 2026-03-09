from beanie import Document, Indexed
from typing import Annotated


class User(Document):
    user_id: Annotated[int, Indexed(unique=True)]
    username: str
    email: str
    roles: list[str] = []

    class Settings:
        name = "users"
