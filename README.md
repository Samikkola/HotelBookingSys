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


***At this stage, the project has a working MVP with EFCore, SQLite, Result Pattern-based error handling, and dependency injection configured.***

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

## Layer Responsibilities (planned)

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

* EF Core implementation (InMemory in MVP)
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

✔ Solution created

✔ Clean Architecture project structure established

✔ Project references configured correctly

✔ Dependency direction verified

✔ Domain entities implemented (Customer, Room, Reservation)

✔ EF Core persistence (SQLite) configured via ApplicationDbContext

✔ EF Core entity models mapped with constraints and relationships

✔ EF Core based repositories implemented for standard core logic

✔ Database seeding configured (creating the required 30 hotel rooms + dummy customer)

✔ Dependency Injection configured for repositories and UseCases

✔ API endpoints created for Customer, Room, and Reservation operations (Create/Get/Update)

✔ Use Cases implemented: Create/Get Customers, Create/Get/Update/Cancel/Complete Reservations, Get All/Available Rooms

✔ DTOs implemented for requests and responses

✔ Result Pattern implemented with HTTP status mapping in controllers

✔ Reservation total price calculation, seasonal pricing logic and domain validation implemented

✔ Reservation overlap validation handling Active bookings only

✔ GitHub Actions CI pipeline configured (.NET Build & Test)

✔ Reservation domain tests written

✔ Swagger integrated and functional

🚧 To Do

Docker not yet configured

Add endpoint to fetch reservation/customer by ID (for CreatedAtAction)

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

*Work in progress – MVP phase ongoing.*
