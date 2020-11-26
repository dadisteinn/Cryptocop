# Cryptocop

Cryptocop was made as the final project in a web API course. 
Cryptocop is a web API for cryptocurrency trade. In additon Cryptocop has two services, an email service for purchase confirmation and a payment service for creditcard validation. 


## Features

#### Cryptocop API
A web API for cryptocurrency trade

- Made using [.NET Core]
- [JWT] web tokens are used for authenticaiton
- [PostgreSQL] database
- [Messari API] is used to get current cryptocurrencie information
- [RabbitMQ] is the message broker used for communicaiton between services

#### Email & Payment service
Two services are used, an email service for purchase confirmation and a payment service for creditcard validation. 

- Made using [Node.js]
- [Mailgun] is used for automated emails


## Tech

- [.NET Core] - Free and open-source, managed computer software framework
- [Entity Framwork] - Modern object-database mapper for .NET
- [JWT] - A compact URL-safe means of representing claims to be transferred between two parties
- [PostgreSQL] - A powerful, open source object-relational database system
- [RabbitMQ] - An open-source message-broker software
- [Node.js] - Event I/O for the backend
- [Mailgun] - An email automation solution that is suitable for developers.
- [Messari API] - A free API for crypto prices, market data metrics, on-chain metrics, and qualitative information.

[.NET Core]: https://docs.microsoft.com/en-us/dotnet/fundamentals/
[Entity Framwork]: https://docs.microsoft.com/en-us/ef/
[JWT]: https://jwt.io/
[PostgreSQL]: https://www.postgresql.org
[Messari API]: https://messari.io/api/docs.
[RabbitMQ]: https://www.rabbitmq.com/
[Node.js]: http://nodejs.org
[Mailgun]: https://www.mailgun.com/
