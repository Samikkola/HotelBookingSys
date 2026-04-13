# Hotel Booking System – Backend

## 📊 Status Badges

[![CI/CD (Build, Test, Deploy)](https://github.com/Samikkola/HotelBookingSys/actions/workflows/dotnet.yml/badge.svg?branch=master)](https://github.com/Samikkola/HotelBookingSys/actions/workflows/dotnet.yml)

## 📌 Project Overview

This project is a backend implementation of a hotel reservation system developed as a final assignment for a Software Architecture course.

The system will be built using:

* **C#**
* **.NET (ASP.NET Core Web API)**
* **Clean Architecture**
* **Entity Framework Core**

The goal is to design and implement a maintainable and scalable REST API for managing hotel customers, rooms, and reservations.


The system is built as a REST API with Clean Architecture, EF Core + SQL Server persistence, Result Pattern-based error handling, and full dependency injection.

---

# 🏗 Architecture

The solution follows **Clean Architecture principles**, where dependencies point inward toward the domain layer.

## Solution Structure

```
HotelBookingSys.slnx

 ├── HotelBookingSys.Domain
 ├── HotelBookingSys.Application
 ├── HotelBookingSys.Infrastructure
 ├── HotelBookingSys.API
 └── HotelBookingSys.Tests
```

---

## Layer Responsibilities

### 🔹 Domain

* Core business entities (Customer, Room, Reservation)
* Enums and value objects
* Repository interfaces
* Domain rules
* No external dependencies

### 🔹 Application

* Use cases 
* DTO's
* Application logic and orchestration
* Service abstractions
* Depends only on Domain

### 🔹 Infrastructure

* EF Core implementation (SQL Server)
* Database configuration
* Repository implementations
* Depends on Application and Domain

### 🔹 API

* REST controllers
* Dependency Injection configuration
* Swagger documentation
* Entry point of the application

### 🔹 Tests

* Unit tests for domain and application logic

---

# 🔗 Dependency Structure

Project references have been configured according to Clean Architecture principles:

```
API → Application → Domain
Infrastructure → Application → Domain
Tests → Application + Domain
```

This ensures:

* The Domain layer has no external dependencies
* Business logic remains independent from frameworks
* Infrastructure details can be replaced without affecting core logic

---

✅ Current Status

The project has a fully functional backend with all core features implemented:
- Clean Architecture structure with Domain, Application, Infrastructure, API, and Tests layers
- EF Core + SQL Server persistence with migrations and database seeding (30 rooms)
- Use cases covering customer management, room availability, and reservation lifecycle (create, update, cancel, complete)
- Seasonal pricing logic and reservation overlap validation
- Result Pattern with HTTP status mapping in controllers
- DTOs for all request and response models
- Customer endpoints for lookup by email/phone, update, and delete
- Reservation filtering support (`customerId`, `roomId`, `status`, `fromDate`, `toDate`)
- GitHub Actions CI/CD pipeline (build, test, EF Core migration, Azure deployment)
- Swagger UI integrated and functional
- Domain unit tests for Reservation, Room and Customer
- Application unit tests for ReservationUseCases


🚧 To Do's

- Expand Result Pattern usage and refine error messages

- Implement max occupancy validation

- Implement authentication in endpoints

- Add support for saving pictures on Rooms

- Expand unit test coverage

- Optionally: add frontend or online booking API

---
# ▶️ Running the Application

### 1. Clone the repository
```bash
git clone https://github.com/<your-username>/HotelBookingSys.git

cd HotelBookingSys
```
### 2. Restore dependencies
```bash
dotnet restore
```

## Local development with Docker

### Prerequisites
- Docker Desktop installed and running
- Ports `8080` (API) and `1433` (SQL Server) available

### 1. Create local environment file
```bash
cp .env.example .env
```

Windows PowerShell alternative:
```powershell
Copy-Item .env.example .env
```

Then edit `.env` if needed:
```env
DB_NAME=hotelbooking-db
DB_PASSWORD={yourStrongPasswordHere}
```

### 2. Start API + SQL Server
```bash
docker compose up --build
```

### 3. Open the API
- Swagger: `http://localhost:8080/swagger`

Notes:
- The API uses `ASPNETCORE_ENVIRONMENT=Development` in compose.
- Startup applies EF Core migrations and seeds initial data.

### 4. Stop containers
```bash
docker compose down
```

To also remove SQL data volume:
```bash
docker compose down -v
```

## Local development without Docker

Use a local SQL Server instance and run the API directly.

1. Configure connection string in `HotelBookingSys.API/appsettings.Development.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=hotelbooking-db;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

or set environment variable:

```bash
ConnectionStrings__DefaultConnection=<your-connection-string>
```

2. Apply migrations:

```bash
dotnet ef database update -p HotelBookingSys.Infrastructure -s HotelBookingSys.API
```

3. Run API:

```bash
dotnet run --project HotelBookingSys.API
```

Swagger will be available at: `http://localhost:5122/swagger`

## Production configuration (Azure)

- Production connection string is **not** stored in code or repository files.
- Configure `ConnectionStrings__DefaultConnection` in Azure App Service Configuration.

## 🚀 CI/CD and Azure Deployment

The repository includes a GitHub Actions workflow at `.github/workflows/dotnet.yml`.

- On `pull_request`: runs build and tests.
- On `push` to `main`/`master`: runs build + tests 
	- If successfull, checks application code for changes and executes EF Core migrations against Azure SQL, and deploys to Azure App Service, if needed.

### Required GitHub Secrets

- `AZURE_WEBAPP_PUBLISH_PROFILE` (download from Azure Portal → App Service → Overview → Download publish profile)
- `AZURE_SQL_CONNECTION_STRING` (Azure SQL connection string used during deployment migrations)

### Azure Target

- App Service name: `app-hotelbookingsys`

### Required Azure Resources (minimal)

- `Azure App Service` (Web App) for hosting the API (`app-hotelbookingsys`)
- `App Service Plan` for the Web App runtime
- `Azure SQL Server` + `Azure SQL Database` for persistence
- Network access from App Service/GitHub Actions to Azure SQL (firewall/VNet rules as needed)

---
# 👨‍💻 Author

Developed as part of a Software Architecture course final project.

---

*Functional backend with SQL Server persistence.*
