import asyncio
from app.core.config import settings
from loguru import logger
from motor.motor_asyncio import AsyncIOMotorClient
from beanie import init_beanie
from app.models import __beanie_models__


async def mongodb_startup() -> None:
    """
    Establishes a connection to the MongoDB database on application startup.

    This function sets the MongoDB client instance in the app state, allowing
    other parts of the application to access the MongoDB connection.
    """
    logger.info("Connecting to MongoDB...")
    retry_count = 0
    max_retries = 5
    retry_delay = 1

    while retry_count < max_retries:
        try:
            client = AsyncIOMotorClient(settings.MONGODB_URL)
            await init_beanie(
                database=client[settings.DATABASE_NAME],
                document_models=__beanie_models__
                )
            logger.info("Connected to MongoDB!")
            return
        
        except Exception as e:
            retry_count += 1
            logger.warning(
                f"Failed to connect to MongoDB. ({settings.MONGODB_URL})"
                f"Retrying in {retry_delay} seconds. ({retry_count}/{max_retries})")
            
            await asyncio.sleep(retry_delay)
            retry_delay *= 2
    
    raise ConnectionError("Could not connect to MongoDB")