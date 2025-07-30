
# 🛍️ SkiNet – E-Commerce API

A scalable and modular e-commerce backend API built with ASP.NET Core, following Clean Architecture and CQRS principles.

## 🚀 Features

- Product catalog management with CRUD operations
- Shopping cart functionality with Redis caching for performance
- Secure JWT-based authentication and authorization
- Payment processing integration with Stripe
- Filtering, sorting, and pagination for product listings
- Validation using Fluent Validation
- Unit Testing for core business logic

## 🧰 Tech Stack

- ASP.NET Core
- Clean Architecture & CQRS
- Redis (Caching)
- Stripe (Payment Gateway)
- Angular (Frontend)
- Fluent Validation
- xUnit / NUnit (Unit Testing)

## 📦 Getting Started

1. Clone the repository:  
   ```bash
   git clone https://github.com/mohammedhany990/Skinet.API.git
````

2. Configure the database connection string and Redis in `appsettings.json`.
3. Set up Stripe API keys in the configuration.
4. Run database migrations and seed data if applicable.
5. Build and run the API project:

   ```bash
   dotnet run
   ```
6. (Optional) Run unit tests:

   ```bash
   dotnet test
   ```

## 🛠️ Project Structure

* **API Layer:** Controllers and request handling
* **Application Layer:** Business logic and CQRS handlers
* **Infrastructure Layer:** Database, Redis, and external services integration
* **Domain Layer:** Core entities and interfaces

## 🔗 Useful Links

* [API Documentation (Swagger)](http://localhost:{PORT}/swagger)
* [GitHub Repository](https://github.com/mohammedhany990/Skinet.API)

