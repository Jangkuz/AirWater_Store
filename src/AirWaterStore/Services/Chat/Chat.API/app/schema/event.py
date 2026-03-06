from datetime import datetime
from pydantic import BaseModel, ConfigDict, Field
from uuid import UUID, uuid4

class IntegrationEvent(BaseModel):
    model_config = ConfigDict(
        populate_by_name=True,
        extra='ignore'
    )
    id: UUID = Field(alias='id')
    occurred_on: datetime = Field(alias='occurredOn')
    event_type: str = Field(alias='eventType')

class UserCreatedEvent(IntegrationEvent):
    user_id: int = Field(alias='userId')
    user_name: str = Field(alias='userName')
    email: str