import time
from fastapi import Request
from loguru import logger
from starlette.middleware.base import BaseHTTPMiddleware


class RequestLoggingMiddleware(BaseHTTPMiddleware):
    async def dispatch(self, request: Request, call_next):
        json_body = (
            await request.json() if request.method in ["POST", "PUT", "PATCH"] else {}
        )
        logger.info(f"Incoming Request: {json_body}")

        start_time = time.time()
        response = await call_next(request)
        process_time = time.time() - start_time

        logger.info(
            f"Completed Request: {request.method} {request.url.path} "
            f"- Status: {response.status_code} - Time: {process_time:.4f}s"
        )

        return response
