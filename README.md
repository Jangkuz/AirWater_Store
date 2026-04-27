# PRN232-Group-Assignment

This is a **personal project/proof of concetp** to learn microservices and use new technologies

Goals:

- Develop a ecommerse web with multiple microservices backends
- Use multiple Db both SQL and NoSQL
- Microservices communicate via RabbitMQ messages
- Add monitoring and scalling using grafana/otel-lgtm stack

## _Note_

Cons (tobe fixed in the future?):

- Business rule is not tight(for learning purposes)
  - If game's price is updated, the game(s) price in cart will not updated
  - Basket's quantity validation is FE based
  - Don't have menu for adding discount, only API
- Update game haven't been sync via Rabbit MQ
- The project don't have a single source of truth. Thus lead to race condition and data lose. Have not found a solution
  - After done `docker compose up -d` for the first time. Delete ordering's and airwaterstore's db due to race condition when seeding.
    - Then rerun air water store docker compose
  - If faced with race condition when seeding database:
    - Keep the table schema
    - Delete the db record
    - Rerun docker compose

## Port

| Service       | Local Port                                             | Docker Port                                            |
| ------------- | ------------------------------------------------------ | ------------------------------------------------------ |
| Yarp Proxy    | <http://localhost:5000> <br/> <https://localhost:5050> | <http://localhost:6000> <br/> <https://localhost:6060> |
| AirWaterStore | <http://localhost:5001> <br/> <https://localhost:5051> | <http://localhost:6001> <br/> <https://localhost:6061> |
| Catalog       | <http://localhost:5002> <br/> <https://localhost:5052> | <http://localhost:6002> <br/> <https://localhost:6062> |
| Basket        | <http://localhost:5003> <br/> <https://localhost:5053> | <http://localhost:6003> <br/> <https://localhost:6063> |
| Discount      | <http://localhost:5004> <br/> <https://localhost:5054> | <http://localhost:6004> <br/> <https://localhost:6064> |
| Ordering      | <http://localhost:5005> <br/> <https://localhost:5055> | <http://localhost:6005> <br/> <https://localhost:6065> |
| Chat          | <https://localhost:5056> | <https://localhost:6066> |

## Databases

| Service       | Database              | Local Port                                             |
| ------------- | --------------------- | ------------------------------------------------------ |
| Yarp Proxy    | N/A                   | N/A                                                    |
| AirWaterStore | PostgreSQL            | <http://localhost:7071>                                |
| Catalog       | PostgreSQL - Document | <http://localhost:7072>                                |
| Basket        | PostgreSQL            | <http://localhost:7073>                                |
| Discount      | SQLite                | N/A                                                    |
| Ordering      | MSSQL                 | <http://localhost:7075>                                |
| Chat          | MongoDB               | <http://localhost:7076>                                |
| Ordering      | Redis Cache           | <http://localhost:6379>                                |
| MessageBroker | RabbitMQ              | <http://localhost:5672> <br/> <http://localhost:15672> |

## _Test account_

- User:
  - Email: <user@gmail.com>
  - Password: 123456As!
- Staff:
  - Email: <staff@gmail.com>
  - Password: 123456As!
