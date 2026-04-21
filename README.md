# HotelBookingSys - Backend

## 📊 Status

[![CI/CD (Build, Test, Deploy)](https://github.com/Samikkola/HotelBookingSys/actions/workflows/dotnet.yml/badge.svg?branch=master)](https://github.com/Samikkola/HotelBookingSys/actions/workflows/dotnet.yml)

## 1. Project overview


`HotelBookingSys` is a RESTful backend for hotel operations, built as a final assignment for a Software Architecture course.

### Tech stack

| Technology | Purpose |
|---|---|
| C# / .NET 10 | Backend framework |
| ASP.NET Core Web API | REST API |
| Entity Framework Core | ORM |
| Docker | Local development |
| GitHub Actions | CI/CD pipeline |
| Azure SQL Database | Production database |
| Azure Blob Storage | Image storage |
| Azure App Service | Cloud hosting |

Live API (Swagger):
- https://app-hotelbookingsys.azurewebsites.net/swagger
## 2. Architecture

The solution follows Clean Architecture: HTTP and infrastructure concerns stay in outer layers, while business rules remain in the inner layers. Use cases in the Application layer orchestrate operations through interfaces, and Infrastructure provides concrete implementations. This keeps business logic testable and independent from frameworks.

```text
┌─────────────────────────────────┐
│           API Layer             │
│  Controllers, Middleware, DI    │
├─────────────────────────────────┤
│       Application Layer         │
│   Use Cases, DTOs, Interfaces   │
├─────────────────────────────────┤
│         Domain Layer            │
│  Entities, Business Rules       │
├─────────────────────────────────┤
│      Infrastructure Layer       │
│  EF Core, Repositories, JWT     │
└─────────────────────────────────┘
```

### Layer responsibilities

**API layer** (`HotelBookingSys.API`) handles request/response concerns only. Controllers call use cases, `BaseController` maps Result objects to HTTP codes, and middleware handles unhandled exceptions with structured logging. Startup config also wires Swagger, JWT middleware, static files, and `/health`.

**Application layer** (`HotelBookingSys.Application`) contains use cases, DTOs, mapping helpers, and Result/PagedResult abstractions. It orchestrates workflows such as reservation creation, overlap checks, room availability search, analytics calculations, and image operations via interfaces.

**Domain layer** (`HotelBookingSys.Domain`) contains entities (`Reservation`, `Room`, `Customer`, `User`, `RoomImage`) and core rules. Examples include reservation date/guest validation, seasonal pricing calculation, and reservation lifecycle transitions (`Active`, `Cancelled`, `Completed`).

**Infrastructure layer** (`HotelBookingSys.Infrastructure`) implements persistence and external integrations: EF Core `ApplicationDbContext`, repository implementations, JWT token generation, local image storage, Azure Blob storage, and idempotent seed data.

### Key design patterns

- **Result Pattern**: Use cases return `Result`/`Result<T>` instead of throwing for expected business failures (validation, not found, conflict, unauthorized). This keeps controller flow explicit and consistent.
- **Strategy Pattern**: `IImageStorageService` abstracts image storage with two implementations: `LocalImageStorageService` for development and `AzureBlobStorageService` for non-development environments.
- **Clean Architecture dependency rule**: Dependencies point inward. API and Infrastructure depend on Application/Domain contracts; Domain does not depend on framework code.

## 3. Features

### Core booking features
- Room management with 5 categories (`Economy`, `Standard`, `Superior`, `JuniorSuite`, `Suite`) seeded by default
- Customer management with full CRUD + lookup by email/phone
- Reservation management: create, patch update, cancel, complete
- Double-booking prevention through overlap checks
- Automatic total price calculation
- Available room search by date range

### Business logic
- Seasonal pricing (+30%) for June-August and December 20-January 6
- Room capacity validation
- Guest count validation during create/update
- Reservation filtering (`customerId`, `roomId`, `status`, `fromDate`, `toDate`) with pagination

### Security
- JWT authentication endpoint (`/api/auth/login`)
- Role claims (`Manager`, `Receptionist`) embedded into issued tokens
- Password hashing with BCrypt
- JWT validation middleware configured globally
- Currently not implemented further, as swagger conflicts with JWT auth.

> Note: Restrictions are currently demonstrated with `/api/auth/authtest` (`Manager` role). Business endpoints do not yet enforce role attributes.

### Analytics
- Monthly revenue report
- Occupancy rate by date range
- Popular room type analytics by date range

### Images
- Upload/delete room images
- Validation for file type (`jpg`, `jpeg`, `png`, `webp`) and max size (`5 MB`)
- Local file storage in development (`wwwroot/images/rooms`)
- Azure Blob Storage in non-development environments

### Infrastructure
- Docker Compose for local API + SQL Server
- CI/CD with build -> test -> migrate -> deploy
- Manual deployment trigger support in workflow (`workflow_dispatch`)
- Health check endpoint (`/health`) with JSON response
- Global exception middleware with logging
- Idempotent database seeding

## 4. Getting started

### Prerequisites
- Docker Desktop
- .NET 10 SDK (for local run/tests)

### Local development with Docker

1. Clone the repository

```sh
git clone https://github.com/<your-username>/HotelBookingSys.git
cd HotelBookingSys
```

2. Create `.env` in solution root:

```env
DB_PASSWORD=strongpassword123!
DB_NAME=hotelbooking-db
Jwt__Secret=dev-secret-key-minimum-32-characters-long!!
Jwt__Issuer=hotelbookingsys-dev
Jwt__Audience=hotelbookingsys-dev
```

3. Start containers:

```sh
docker compose up --build
```

4. Open Swagger:
- http://localhost:8080/swagger

5. In `Development`, migrations and seed data run automatically at startup.

### Running tests

```sh
dotnet test
```

## 5. API reference

### Authentication

`POST /api/auth/login` validates credentials and returns a JWT token, expiration, role, and name fields.


Seeded test credentials:

| Role | Email | Password |
|---|---|---|
| Manager | manager@hotellakeview.fi | Manager123! |
| Receptionist | receptionist@hotellakeview.fi | Staff123! |

### Endpoints

| Method | Endpoint | Description | Access |
|---|---|---|---|
| POST | `/api/auth/login` | Login and get JWT token | Public |
| GET | `/api/auth/authtest` | Auth test endpoint | Manager |
| GET | `/api/customers` | List customers | Public |
| POST | `/api/customers` | Create customer | Public |
| GET | `/api/customers/{id}` | Get customer by id | Public |
| GET | `/api/customers/by-email?email=` | Find customer by email | Public |
| GET | `/api/customers/by-phone?phone=` | Find customer by phone | Public |
| PUT | `/api/customers/{id}` | Update customer | Public |
| DELETE | `/api/customers/{id}` | Delete customer | Public |
| GET | `/api/rooms` | List all rooms | Public |
| GET | `/api/rooms/available?checkInDate=&checkOutDate=` | Get available rooms | Public |
| POST | `/api/rooms/{id}/images` | Upload room image | Public |
| DELETE | `/api/rooms/{id}/images/{imageId}` | Delete room image | Public |
| GET | `/api/reservations` | List reservations (paged + filters) | Public |
| GET | `/api/reservations/{id}` | Get reservation by id | Public |
| POST | `/api/reservations` | Create reservation | Public |
| PATCH | `/api/reservations/{id}` | Partially update reservation | Public |
| PUT | `/api/reservations/{id}/cancel` | Cancel reservation | Public |
| PUT | `/api/reservations/{id}/complete` | Complete reservation | Public |
| GET | `/api/analytics/revenue?year=` | Monthly revenue | Public |
| GET | `/api/analytics/occupancy?from=&to=` | Occupancy report | Public |
| GET | `/api/analytics/popular-room-types?from=&to=` | Popular room types | Public |
| GET | `/health` | Health check | Public |

### Example: Get JWT token and get response from Authorized endpoint (PowerShell)

```powershell
# Login
$loginResponse = Invoke-RestMethod `
    -Uri "http://localhost:8080/api/auth/login" `
    -Method POST `
    -ContentType "application/json" `
    -Body '{"email":"receptionist@hotellakeview.fi","password":"Staff123!"}'

- #Set the login token to a variable
$token = $loginResponse.token


#Use the token to call an authorized endpoint
Invoke-RestMethod `
    -Uri "http://localhost:8080/api/auth/authtest" `
    -Method Get `
    -ContentType "application/json" `
    -Headers @{Authorization = "Bearer $token"} `
```

## 6. Azure deployment

Live URL:
- https://app-hotelbookingsys.azurewebsites.net/swagger

GitHub Actions workflow (`.github/workflows/dotnet.yml`):
1. Build and test on pull requests and pushes to `master`/`main`
2. Deploy automatically on push to `master`/`main` when deploy-relevant paths change
3. Deploy can also be triggered manually via `workflow_dispatch`
4. During deploy, EF Core migrations run against Azure SQL before App Service deployment

Required App Service / deployment variables:

| Variable | Description |
|---|---|
| `ConnectionStrings__DefaultConnection` | Azure SQL connection string |
| `Jwt__Secret` | JWT signing secret (min 32 chars) |
| `Jwt__Issuer` | JWT issuer |
| `Jwt__Audience` | JWT audience |
| `AzureStorage__ConnectionString` | Azure Blob Storage connection string |

## 7. Advanced features explained

### Result Pattern
Use cases return explicit success/failure objects instead of relying on exceptions for normal business errors. This improves API consistency because controllers map known error codes (`Validation`, `NotFound`, `Conflict`, etc.) into correct HTTP responses.

### Strategy Pattern (image storage)
Image handling is abstracted behind `IImageStorageService`. The implementation switches by environment: development uses local file storage, while non-development uses Azure Blob Storage, without changing use case logic.

### JWT authentication with role claims
`LoginUseCase` verifies credentials against seeded users and BCrypt password hashes, then `JwtTokenService` issues a signed token with role and identity claims. Token validation is configured in startup and can be enforced per endpoint via authorization attributes.

### Seasonal pricing algorithm
`Reservation` calculates price per night and applies a 30% multiplier during summer (June-August) and the Christmas/New Year period (Dec 20-Jan 6). This keeps pricing rules in the domain where they are easy to test.

### CI/CD with automatic migrations
The workflow restores, builds, and tests first. For deployment runs, it applies EF Core migrations to Azure SQL (with retries) and then deploys the published API package to Azure App Service.

### Docker Compose setup
`docker-compose.yml` runs SQL Server and API together with a health check dependency. This gives a reproducible local environment close to production integration behavior.

### Health check endpoint
`/health` is mapped with a JSON response that reports overall status, individual checks, and duration. Database readiness is included via `AddDbContextCheck<ApplicationDbContext>("database")`.

### Idempotent database seeding
Startup seeding inserts baseline rooms, one test customer, and staff users only when missing. This allows repeated startup runs without duplicate seed data.

## 8. Project structure

```text
HotelBookingSys/
├── HotelBookingSys.Domain/          # Entities, enums, and repository contracts
├── HotelBookingSys.Application/     # Use cases, DTOs, mappings, Result abstractions
├── HotelBookingSys.Infrastructure/  # EF Core, repositories, storage and JWT services, seeders
├── HotelBookingSys.API/             # Controllers, middleware, startup configuration
└── HotelBookingSys.Tests/           # Domain and application unit tests
```


### Next steps if the project continued
- Add authorization attributes to business endpoints by role.
- Add integration tests (API + database) alongside unit tests.
- Expand observability (structured logs, metrics, tracing).
- Add richer pricing rules (weekend/event rates, promo codes).
- Add MediatR for decoupling controllers from use cases.


Developed as part of a Software Architecture course final project.
