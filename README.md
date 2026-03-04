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

✅ Current Status

✔ Solution created
✔ Clean Architecture structure implemented
✔ Project references configured correctly
✔ Domain entities implemented
✔ Business validation inside Domain
✔ Reservation price calculated inside Domain
✔ Reservation unit tests written and passing

🚧 Application use cases not yet implemented
🚧 Database not yet configured
🚧 API endpoints not yet implemented
🚧 Docker configuration not yet added

---

🎯 Next Steps (MVP Roadmap)

Add more Domain unit tests?

1. Implement Application layer:

	- CreateReservation use case

	- Repository interfaces

	- ReservationService for overlap validation

2. Configure EF Core with InMemory database

3. Implement Infrastructure repositories

4. Create basic API endpoints:

	- Create Customer

	- Create Room

	- Create Reservation

	- Get Reservations

5. Add reservation overlap validation (business rule)

6. Dockerize the application

7. Extend unit tests to cover Application layer

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
