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


***At this stage, the project has a working MVP with EFCore, SQLite, basic API endpoints, and dependency injection configured.***

---

# 🏗 Architecture

The solution follows **Clean Architecture principles**, where dependencies point inward toward the domain layer.

## Solution Structure

```
HotelBookingSystem.sln

 ├── HotelBooking.Domain
 ├── HotelBooking.Application
 ├── HotelBooking.Infrastructure
 ├── HotelBooking.API
 └── HotelBooking.Tests
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

✔ Reservation total price calculation and domain validation implemented

✔ Reservation overlap validation handling Active bookings only

✔ GitHub Actions CI pipeline configured (.NET Build & Test)

✔ Reservation domain tests written

✔ Swagger integrated and functional

🚧 To Do

Docker not yet configured

Error handling pattern (Result Pattern) to be implemented

---

# 🎯 Next Steps (Might change as development progresses)

Implement Result Pattern for robust error handling

Implement advanced pricing logic (seasonal pricing, max occupancy validation)

Expand unit test coverage

Dockerize the application

Optionally: add frontend or online booking API

---

# 🎓 Purpose of This Structure

The goal of starting with a clean architectural foundation is to:

* Ensure proper separation of concerns
* Maintain testability
* Prevent framework dependency leakage into the domain
* Support future extensions (e.g., online booking system)


# 👨‍💻 Author

Developed as part of a Software Architecture course final project.

---

*Work in progress – MVP phase ongoing.*
