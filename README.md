# eShop Microservices - 0.5 YOE Evolution

An evolved version of my original **eShop Microservices** project, redesigned and extended after gaining six months of professional software engineering experience.

The original project established the foundation of a distributed e-commerce system using ASP.NET Core, Vertical Slice Architecture, DDD, CQRS, PostgreSQL with Marten, Redis, SQL Server, gRPC, RabbitMQ, MassTransit, YARP API Gateway, and Docker Compose.

This repository evolves that foundation toward a more production-oriented, cloud-native distributed system. The main focus is not simply adding more technologies, but improving architectural decisions, reliability, data consistency, observability, deployment automation, and failure handling.

The system is progressively redesigned with native document databases, reliable event-driven communication, distributed transaction patterns, modern frontend architecture, automated CI/CD pipelines, and Kubernetes-based cloud deployment.

## Contributors:
- **Truong Nhat Anh** ([ShouraiNoPurogurama](https://github.com/ShouraiNoPurogurama)) - Sole developer of the application, responsible for the full implementation.

- **Mehmet Ozkaya** ([mehmetozkaya](https://github.com/mehmetozkaya)) - Provided initial groundwork and mentorship.


## Project Evolution

The original implementation included:

- Catalog, Basket, Discount, and Ordering microservices
- Vertical Slice Architecture
- Domain-Driven Design and Clean Architecture for complex business domains
- CQRS with MediatR
- PostgreSQL and Marten as a transactional document database
- Redis distributed caching
- SQL Server and Entity Framework Core
- Synchronous gRPC communication
- Asynchronous RabbitMQ communication with MassTransit
- YARP API Gateway
- Rate limiting
- Razor-based Shopping Web UI
- Docker Compose containerization

This evolved version extends and redesigns the system with:

- Migration from Marten/PostgreSQL DocumentDB to MongoDB
- Native document-oriented persistence for the Catalog domain
- Improved polyglot persistence strategy
- Payment Microservice and distributed payment workflows
- Transactional Outbox Pattern for reliable event publishing
- Idempotent message processing
- Saga Pattern for distributed business workflows
- Improved event-driven architecture
- Explicit failure handling and compensation workflows
- Resilience patterns for distributed communication
- Modern Angular frontend replacing the original Razor Web UI
- Improved authentication and authorization architecture
- Distributed observability and monitoring
- Container orchestration with Kubernetes
- Helm-based application deployment
- Azure Kubernetes Service deployment
- Fully automated CI/CD pipelines on Azure
- Cloud-native configuration and deployment strategies
- Improved automated testing and system reliability

## Architecture Philosophy

The project intentionally avoids enforcing a single architectural style across every microservice.

Each service is designed according to its domain complexity, data access patterns, and operational requirements.

Simple, data-oriented services use lightweight Vertical Slice Architecture, while complex business domains use Domain-Driven Design and Clean Architecture to enforce domain boundaries and maintainability.

The system explores practical architectural decisions involving:

- Vertical Slice Architecture
- Clean Architecture
- Domain-Driven Design
- CQRS
- Event-Driven Architecture
- Database per Service
- Polyglot Persistence
- Synchronous and Asynchronous Communication
- Eventual Consistency
- Distributed Transactions
- Reliable Messaging
- Cloud-Native Deployment

## Microservices

### Catalog Microservice

- ASP.NET Core Minimal APIs
- Vertical Slice Architecture
- CQRS with MediatR
- FluentValidation Pipeline Behaviors
- Carter Minimal API endpoints
- MongoDB native document database
- Flexible document-oriented data modeling
- Optimized document queries and indexing
- Structured logging
- Global exception handling
- Health checks

The Catalog service is migrated from Marten/PostgreSQL to MongoDB to explore native document-oriented persistence, schema flexibility, independent scaling, and document-centric query patterns.

### Basket microservice which includes;
* ASP.NET 8 Web API application, Following REST API principles, CRUD
* Using **Redis** as a **Distributed Cache** over basketdb
* Implements Proxy, Decorator and Cache-aside patterns
* Consume Discount **Grpc Service** for inter-service sync communication to calculate product final price
* Publish BasketCheckout Queue with using **MassTransit and RabbitMQ**
  
### Discount microservice which includes;
* ASP.NET **Grpc Server** application
* Build a Highly Performant **inter-service gRPC Communication** with Basket Microservice
* Exposing Grpc Services with creating **Protobuf messages**
* Entity Framework Core ORM — SQLite Data Provider and Migrations to simplify data access and ensure high performance
* **SQLite database** connection and containerization

### Microservices Communication
* Sync inter-service **gRPC Communication**
* Async Microservices Communication with **RabbitMQ Message-Broker Service**
* Using **RabbitMQ Publish/Subscribe Topic** Exchange Model
* Using **MassTransit** for abstraction over RabbitMQ Message-Broker system
* Publishing BasketCheckout event queue from Basket microservices and Subscribing this event from Ordering microservices	
* Create **RabbitMQ EventBus.Messages library** and add references Microservices

### Ordering Microservice
* Implementing **DDD, CQRS, and Clean Architecture** with using Best Practices
* Developing **CQRS with using MediatR, FluentValidation and Mapster packages**
* Consuming **RabbitMQ** BasketCheckout event queue with using **MassTransit-RabbitMQ** Configuration
* **SqlServer database** connection and containerization
* Using **Entity Framework Core ORM** and auto migrate to SqlServer when application startup
	
### Yarp API Gateway Microservice
* Develop API Gateways with **Yarp Reverse Proxy** applying Gateway Routing Pattern
* Yarp Reverse Proxy Configuration; Route, Cluster, Path, Transform, Destinations
* **Rate Limiting** with FixedWindowLimiter on Yarp Reverse Proxy Configuration

### WebUI ShoppingApp Microservice
* ASP.NET Core Web Application with Bootstrap 4 and Razor template
* Call **Yarp APIs with Refit HttpClientFactory**

#### Docker Compose establishment with all microservices on docker;
* Containerization of microservices
* Containerization of databases
* Override Environment variables

## Run The Project
You will need the following tools:

* [Visual Studio 2022](https://visualstudio.microsoft.com/downloads/)
* [.Net Core 8 or later](https://dotnet.microsoft.com/download/dotnet-core/8)
* [Docker Desktop](https://www.docker.com/products/docker-desktop)

### Installing
Follow these steps to get your development environment set up: (Before Run Start the Docker Desktop)
1. Clone the repository
2. Once Docker for Windows is installed, go to the **Settings > Advanced option**, from the Docker icon in the system tray, to configure the minimum amount of memory and CPU like so:
* **Memory: 4 GB**
* CPU: 2
3. At the root directory of solution, select **docker-compose** and **Set a startup project**. **Run docker-compose without debugging on visual studio**.
  Or you can go to root directory which include **docker-compose.yml** files, run below command:
```csharp
docker-compose -f docker-compose.yml -f docker-compose.override.yml up -d
```

4. Wait for docker compose all microservices. That’s it! (some microservices need extra time to work so please wait if not worked in first shut)

5. Launch **Shopping Web UI -> https://localhost:6065** in your browser to view index page. You can use Web project in order to **call microservices over Yarp API Gateway**. When you **checkout the basket** you can follow **queue record on RabbitMQ dashboard**.

**Special appreciation to Mehmet Ozkaya** - **Initial work** - [mehmetozkaya](https://github.com/mehmetozkaya)
