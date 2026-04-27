# AirWater Store Architecture

This document serves as a high-level overview of the microservices architecture, routing, and database integrations for the AirWater Store project. It is intended to provide quick context to AI coding assistants and developers.

## Overview
AirWater Store is a microservices-based e-commerce platform built with .NET 8/9. It utilizes various communication protocols (REST, RabbitMQ) and persists data across multiple purpose-built databases.

## Services & Ports

| Service       | Local Port                                             | Docker Port                                            | Responsibility |
| ------------- | ------------------------------------------------------ | ------------------------------------------------------ | -------------- |
| **Yarp Proxy**| <http://localhost:5000> / <https://localhost:5050>     | <http://localhost:6000> / <https://localhost:6060>     | API Gateway & Routing |
| **AirWaterStore**| <http://localhost:5001> / <https://localhost:5051>  | <http://localhost:6001> / <https://localhost:6061>     | Web Frontend / BFF |
| **Catalog**   | <http://localhost:5002> / <https://localhost:5052>     | <http://localhost:6002> / <https://localhost:6062>     | Product management & Inventory |
| **Basket**    | <http://localhost:5003> / <https://localhost:5053>     | <http://localhost:6003> / <https://localhost:6063>     | User shopping cart |
| **Discount**  | <http://localhost:5004> / <https://localhost:5054>     | <http://localhost:6004> / <https://localhost:6064>     | Promo codes & Price reductions |
| **Ordering**  | <http://localhost:5005> / <https://localhost:5055>     | <http://localhost:6005> / <https://localhost:6065>     | Order processing & checkout |
| **Chat**      | <https://localhost:5056>                               | <https://localhost:6066>                               | Live communication |

## Databases

The system employs a polyglot persistence approach:

| Service       | Database Engine       | Local Port / Connection | Notes |
| ------------- | --------------------- | ----------------------- | ----- |
| AirWaterStore | PostgreSQL            | `localhost:7071`        | Main relational store |
| Catalog       | PostgreSQL (Document) | `localhost:7072`        | JSON/Document mapping |
| Basket        | PostgreSQL            | `localhost:7073`        | Shopping cart persistence |
| Discount      | SQLite                | N/A                     | Lightweight, local file |
| Ordering      | MSSQL                 | `localhost:7075`        | Enterprise relational db |
| Ordering      | Redis                 | `localhost:6379`        | Distributed Cache |
| Chat          | MongoDB               | `localhost:7076`        | NoSQL document store |
| MessageBroker | RabbitMQ              | `5672` (Admin `15672`)  | Event bus / Pub-sub |

## Known Caveats & Tech Debt
- **Race conditions during seeding**: If you experience a race condition during `docker compose up -d`, delete the `ordering` and `airwaterstore` databases while keeping the table schema, then re-run the docker compose.
- **Data Inconsistencies**: There is currently no single source of truth for certain product properties across services. For instance, game price updates are not yet synchronized via RabbitMQ to carts.
- **Frontend Validation**: Basket quantity validation is currently only enforced on the frontend.
