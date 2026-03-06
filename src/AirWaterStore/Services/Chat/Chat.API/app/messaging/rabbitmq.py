import asyncio
import aio_pika, json
from app.core.config import settings
from .handler import handlers

_connection: aio_pika.RobustConnection | None = None
_channel: aio_pika.Channel | None = None

async def rabbitmq_startup() -> None:
    global _connection, _channel
    _connection = await aio_pika.connect_robust(settings.MESSAGE_BROKER_HOST)
    if _connection.connected:
        print("Connected to RabbitMQ!")

    await start_message_consumer()

# def get_channel() -> aio_pika.Channel:
#     if not _channel:
#         raise RuntimeError("RabbitMQ is not connected!")
#     return _channel

async def rabbitmq_shutdown() -> None:
    if _connection:
        await _connection.close()

async def start_message_consumer() -> None:
    async with _connection:
        channel = await _connection.channel()
        await channel.set_qos(prefetch_count=100)

        exchange = await channel.declare_exchange(
            "BuildingBlocks.Messaging.Events:UserCreatedEvent",
            aio_pika.ExchangeType.FANOUT,
            durable=True,
        )

        queue = await channel.declare_queue(
            "python.user-created",
            durable=True,
        )

        await queue.bind(exchange)

        await queue.consume(on_message)

        try:
            await asyncio.Future()
        finally:
            await _connection.close()

async def on_message(message: aio_pika.IncomingMessage):
    async with message.process():
        envelope: dict = json.loads(message.body.decode())

        event_data: dict = envelope.get("message")

        if not event_data:
            print("Invald MassTransit message!")
            return
        
        event_type = get_event_name(envelope)

        handler = handlers.get(event_type)

        print(f"Event handler directing {event_type}")

        if not handler:
            print(f"No event handler for {event_type}")
            return
        
        await handler(event_data)

def get_event_name(envelope: dict) -> str:
    types: list[str] = envelope.get("messageType", [])

    if not types:
        return ""
    
    return types[0].split(":")[-1]