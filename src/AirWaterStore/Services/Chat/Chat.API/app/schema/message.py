from datetime import datetime
from typing import Optional, List
from pydantic import BaseModel, ConfigDict, AliasGenerator
from pydantic.alias_generators import to_camel


class MessageResponse(BaseModel):
    message_id: str
    chat_room_id: str
    user_id: int
    content: str
    sent_at: datetime

    model_config = ConfigDict(
        alias_generator=AliasGenerator(serialization_alias=to_camel),
    )


class MessageCreateRequest(BaseModel):
    user_id: int
    chat_room_id: str
    content: str

    model_config = ConfigDict(
        alias_generator=AliasGenerator(validation_alias=to_camel),
        populate_by_name=True,
    )


class MessageResponseEnvelope(BaseModel):
    message: MessageResponse
    model_config = ConfigDict(
        alias_generator=AliasGenerator(serialization_alias=to_camel),
    )


class MessageListResponseEnvelope(BaseModel):
    messages: List[MessageResponse]

    model_config = ConfigDict(
        alias_generator=AliasGenerator(serialization_alias=to_camel),
    )
