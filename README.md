# Hotel Booking System – Backend

## 📊 Status

[![CI/CD (Build, Test, Deploy)](https://github.com/Samikkola/HotelBookingSys/actions/workflows/dotnet.yml/badge.svg?branch=master)](https://github.com/Samikkola/HotelBookingSys/actions/workflows/dotnet.yml)

---

## 📌 Project Overview

`HotelBookingSys` is a RESTful backend for hotel operations, built as a final assignment for a Software Architecture course.

The API supports:
- customer management
- room discovery and availability
- reservation lifecycle handling
- analytics reporting
- room image upload and storage

### Tech Stack

- `C#`
- `.NET 10` / `ASP.NET Core Web API`
- `Entity Framework Core` + `SQL Server`
- `Swagger / OpenAPI`
- `xUnit`, `Moq`, `FluentAssertions`

---

## 🏗 Architecture

This solution follows **Clean Architecture** with strict inward dependencies:

- API layer handles HTTP concerns only.
- Application layer contains use cases and orchestration.
- Domain layer contains business entities, rules, and repository contracts.
- Infrastructure layer contains EF Core, repository implementations, and storage integrations.

### Dependency Rule

```text
API → Application → Domain
Infrastructure → Application → Domain
Tests → Application + Domain
```

No external framework concerns are placed in Domain.

### Solution Structure

```text
HotelBookingSys.slnx

├── .github/
│   └── workflows/
├── HotelBookingSys.API/
│   ├── Controllers/
│   ├── Properties/
│   
├── HotelBookingSys.Application/
│   ├── Common/
│   │   └── Result/
│   ├── DTOs/
│   │   ├── AnalyticDtos/
│   │   ├── CustomerDtos/
│   │   ├── ReservationDtos/
│   │   └── RoomDtos/
│   ├── Interfaces/
│   └── UseCases/
│       ├── Analytics/
│       ├── Customers/
│       ├── Reservations/
│       └── Rooms/
├── HotelBookingSys.Domain/
│   ├── Entities/
│   ├── Enums/
│   └── Interfaces/
├── HotelBookingSys.Infrastructure/
│   ├── Data/
│   ├── DepencyInjection/
│   ├── Migrations/
│   ├── Repositories/
│   ├── Services/
│   └── Seeders/
├── HotelBookingSys.Tests/
│   ├── Application/
│   └── Domain/
├── docker-compose.yml
├── .env.example
└── README.md
```

---

## ✅ Implemented Functionality

- Clean Architecture structure with Result Pattern response handling
- Customer CRUD + lookup by email/phone
- Room listing and availability search
- Reservation create/update/cancel/complete flows
- Reservation filtering (`customerId`, `roomId`, `status`, `fromDate`, `toDate`)
- Analytics endpoints
- Room image management
- EF Core migrations and startup seeding
- Swagger UI for endpoint testing
- CI/CD with build, test, migration, and Azure deployment stages

---

## 🧭 API Overview

Base URL (local):
- `http://localhost:5122` (direct run)
- `http://localhost:8080` (Docker compose)

Swagger UI:
- `http://localhost:5122/swagger`
- `http://localhost:8080/swagger`

### Main Endpoint Groups

- `api/customers`
- `api/rooms`
- `api/reservations`
- `api/analytics`


> In Development, local image files are served from `wwwroot/images/rooms`.

---

## ▶️ Run the Application

## 1) Clone and restore

```bash
git clone https://github.com/<your-username>/HotelBookingSys.git
cd HotelBookingSys
dotnet restore
```

## 2) Run with Docker (recommended for local full stack)

### Prerequisites
- Docker Desktop running
- ports `8080` and `1433` available

Create environment file:

```bash
cp .env.example .env
```

PowerShell alternative:

```powershell
Copy-Item .env.example .env
```

Start containers:

```bash
docker compose up --build
```

Open Swagger:
- `http://localhost:8080/swagger`

Stop:

```bash
docker compose down
```

Remove DB volume too:

```bash
docker compose down -v
```

## 3) Run without Docker

Use local SQL Server.

Set connection string in `HotelBookingSys.API/appsettings.Development.json`:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost;Database=hotelbooking-db;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

Or environment variable:

```bash
ConnectionStrings__DefaultConnection=<your-connection-string>
```

Apply migrations:

```bash
dotnet ef database update -p HotelBookingSys.Infrastructure -s HotelBookingSys.API
```

Run API:

```bash
dotnet run --project HotelBookingSys.API
```

---

## ☁️ Azure / Production Configuration

Set these values in Azure App Service Configuration:

- `ConnectionStrings__DefaultConnection`
- `AzureStorage__ConnectionString`

Room image storage strategy:
- `Development` → local file storage (`wwwroot/images/rooms`)
- non-Development (e.g. Production) → Azure Blob Storage (coming soon...)

---

## 🚀 CI/CD

Workflow file:
- `.github/workflows/dotnet.yml`

Behavior:
- On `pull_request`: build + tests
- On `push` to `main`/`master`: build + tests + conditional migration/deploy pipeline

Required GitHub secrets:
- `AZURE_WEBAPP_PUBLISH_PROFILE`
- `AZURE_SQL_CONNECTION_STRING`

Target App Service:
- `app-hotelbookingsys`

---

## 🧪 Testing

Test project:
- `HotelBookingSys.Tests`

Includes:
- Domain unit tests
- Application use case unit tests

Run tests:

```bash
dotnet test
```

---

## 👨‍💻 Author

Developed as part of a Software Architecture course final project.

*Functional backend with SQL Server persistence.*
