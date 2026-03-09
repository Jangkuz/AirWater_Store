from pydantic_settings import BaseSettings, SettingsConfigDict
from pydantic import Field


class Settings(BaseSettings):
    # App Settings
    APP_NAME: str = "Chat Microservice"
    DEBUG: bool = False

    # MongoDB Settings
    # If MONGODB_URL isn't in .env, it defaults to the string below
    MONGODB_URL: str = Field(default="mongodb://localhost:27017")
    DATABASE_NAME: str = "ChatDb"
    CHATROOM_COLLECTION_NAME: str = "ChatRooms"
    MESSAGES_COLLECTION_NAME: str = "Messages"

    # RabbitMQ settings
    MESSAGE_BROKER_HOST: str = "amqp://guest:guest@localhost:5672"

    # Security
    SECRET_KEY: str = "development-secret-key"

    # Pydantic Configuration
    # This tells Pydantic to read from a .env file
    model_config = SettingsConfigDict(env_file=".env", extra="ignore")


# Instantiate the settings once so they can be imported elsewhere
settings = Settings()
