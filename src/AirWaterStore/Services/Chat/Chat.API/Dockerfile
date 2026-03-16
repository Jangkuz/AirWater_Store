FROM python:3.11-slim AS base
WORKDIR /app
EXPOSE 8000

# This stage is used to build the service project
FROM python:3.11-slim AS build
WORKDIR /src

# Prevent Python from writing .pyc files (keeps image clean)
ENV PYTHONDONTWRITEBYTECODE=1
ENV PYTHONUNBUFFERED=1

RUN apt-get update && apt-get install -y \
    gcc \
    python3-dev \
    && rm -rf /var/lib/apt/lists/*

COPY Services/Chat/Chat.API/requirements.txt .
RUN pip install --no-cache-dir --prefix=/install -r requirements.txt

FROM base AS final
WORKDIR /app

COPY --from=build /install /usr/local

COPY Services/Chat/Chat.API .

ENTRYPOINT ["uvicorn", "main:app", "--host", "0.0.0.0", "--port", "8000"]