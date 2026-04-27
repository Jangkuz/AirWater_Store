from typing import Optional
from pydantic import BaseModel, ConfigDict, AliasGenerator
from pydantic.alias_generators import to_camel


class ChatRoomResponse(BaseModel):
    chat_room_id: str
    customer_id: int
    staff_id: Optional[int] = None

    model_config = ConfigDict(
        alias_generator=AliasGenerator(serialization_alias=to_camel),
    )


class ChatRoomEnvelope(BaseModel):
    chat_room: ChatRoomResponse

    model_config = ConfigDict(
        alias_generator=AliasGenerator(serialization_alias=to_camel),
    )


class ChatRoomCreateRequest(BaseModel):
    customer_id: int


class ChatRoomUpdateStaffRequest(BaseModel):
    staff_id: int
