from loguru import logger
from app.models.user import User
from app.schema.event import UserCreatedEvent

handlers: dict[str, callable] = {}

def register(event_type: str):
    def decorator(func):
        handlers[event_type] = func
        return func
    return decorator

@register("UserCreatedEvent")
async def handle_user_created(data: dict):
    created_user = UserCreatedEvent.model_validate(data)
    if not created_user:
        logger.error("Invalid user data format")

    existing_user = await User.find_one(User.user_id == created_user.user_id)
    if not existing_user:
        await User.insert_one(
            User(
                user_id=created_user.user_id,
                username=created_user.user_name,
                email=created_user.email,
                roles=created_user.roles
            )
        )
