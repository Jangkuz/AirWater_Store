import asyncio
import aio_pika, json
from loguru import logger
from app.core.config import settings
from .handler import handlers

_connection: aio_pika.RobustConnection | None = None
_channel: aio_pika.Channel | None = None


async def rabbitmq_startup() -> None:
    global _connection, _channel

    retry_count = 0
    max_retries = 5
    retry_delay = 5

    while retry_count < max_retries:
        try:
            logger.info("Connecting to RabbitMQ...")
            _connection = await aio_pika.connect_robust(
                f"{settings.MESSAGE_BROKER_HOST}?name={settings.APP_NAME}"
            )
            if _connection.connected:
                logger.info("Connected to RabbitMQ!")
                await start_message_consumer()

            return
        except Exception as e:
            retry_count += 1
            logger.error(f"Failed to connect to RabbitMQ: {e}")
            logger.warning(
                f"Failed to connect to RabbitMQ. "
                f"Retrying in {retry_delay} seconds. ({retry_count}/{max_retries})"
            )

            await asyncio.sleep(retry_delay)
            retry_delay *= 2

    raise aio_pika.exceptions.AMQPConnectionError()


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
            logger.warning("Invald MassTransit message!")
            return

        event_type = get_event_name(envelope)

        handler = handlers.get(event_type)

        logger.log(f"Event handler directing {event_type}")

        if not handler:
            logger.log(f"No event handler for {event_type}")
            return

        await handler(event_data)


def get_event_name(envelope: dict) -> str:
    types: list[str] = envelope.get("messageType", [])

    if not types:
        return ""

    return types[0].split(":")[-1]
