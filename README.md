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

At this stage, the project structure and architectural foundation have been created. Business logic and functionality are not yet implemented, only planned.

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

## Layer Responsibilities (Planned)

### 🔹 Domain

* Core business entities (Customer, Room, Reservation)
* Enums and value objects
* Domain rules
* No external dependencies

### 🔹 Application

* Use cases (commands & queries)
* Repository interfaces
* Service abstractions
* Depends only on Domain

### 🔹 Infrastructure

* EF Core implementation
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

# ✅ Current Status

✔ Solution created
✔ Clean Architecture project structure established
✔ Project references configured correctly
✔ Dependency direction verified

🚧 No business logic implemented yet
🚧 No database configured yet
🚧 No API endpoints implemented yet

---

# 🎯 Next Steps (Might change as development progresses)

1. Implement Domain entities
2. Define repository interfaces in Application layer
3. Configure EF Core with InMemory database
4. Implement basic CRUD functionality
5. Add reservation overlap validation
6. Add pricing logic
7. Add unit tests
8. Dockerize the application

---

# 🎓 Purpose of This Structure

The goal of starting with a clean architectural foundation is to:

* Ensure proper separation of concerns
* Maintain testability
* Prevent framework dependency leakage into the domain
* Support future extensions (e.g., online booking system)

---

*Project is currently in initial setup phase.*


---

# 👨‍💻 Author

Developed as part of a Software Architecture course final project.

---

*Work in progress – MVP phase ongoing.*
