# Hotel Booking System – Backend

## 📌 Project Overview

This project is a backend implementation of a hotel reservation system developed as a final assignment for a Software Architecture course.

The system will be built using:

* **C#**
* **.NET (ASP.NET Core Web API)**
* **Clean Architecture**
* **Entity Framework Core**
* **Docker** 

The goal is to design and implement a maintainable and scalable REST API for managing hotel customers, rooms, and reservations.


The system is built as a REST API with Clean Architecture, EF Core + SQLite persistence, Result Pattern-based error handling, and full dependency injection.

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
* Domain rules
* No external dependencies

### 🔹 Application

* Use cases 
* Repository interfaces
* Service abstractions
* Depends only on Domain

### 🔹 Infrastructure

* EF Core implementation (SQLite)
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
- EF Core + SQLite persistence with migrations and database seeding (30 rooms)
- Full CRUD/action endpoints for Customers, Rooms, and Reservations
- Use cases covering customer management, room availability, and reservation lifecycle (create, update dates, cancel, complete)
- Seasonal pricing logic and reservation overlap validation
- Result Pattern with HTTP status mapping in controllers
- DTOs for all request and response models
- GitHub Actions CI pipeline (build & test)
- Swagger UI integrated and functional
- Domain unit tests for Reservation logic

🚧 To Do

Docker not yet configured

---

# 🎯 Next Steps (Might change as development progresses)

Expand Result Pattern usage and refine error messages

Implement max occupancy validation

Expand unit test coverage

Dockerize the application

Optionally: add frontend or online booking API

---
# ▶️ Running the Application

## Local setup
### 1. Clone the repository
```bash
git clone https://github.com/<your-username>/HotelBookingSys.git

cd HotelBookingSys
```
### 2. Restore dependecies
```bash
dotnet restore
```
### 3. Database setup (EF Core + SQLite)
The project uses SQLite with Entity Framework Core.

#### Add migration 
```bash
dotnet ef migrations add InitialCreate -p HotelBookingSys.Infrastructure -s HotelBookingSys.API
```
* -p = project containing DbContext (Infrastructure)
* -s = startup project (API)

#### Apply migrations (creates database)
```bash
dotnet ef database update -p HotelBookingSys.Infrastructure -s HotelBookingSys.API
```
* Create the SQLite database (hotelbooking.db)
* Apply all schema changes
* If schema changes, run new migrations according these instructions
### 4. Start the application
``` bash
dotnet run --project HotelBookingSys.API
```
### Swager UI will be available at: *http://localhost:5122/swagger*

---
# 👨‍💻 Author

Developed as part of a Software Architecture course final project.

---

*Functional backend – Docker integration pending.*
