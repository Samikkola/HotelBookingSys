using FluentAssertions;
using HotelBookingSys.Domain.Entities;
using HotelBookingSys.Domain.Enums;

namespace HotelBookingSys.Tests.Domain;

public class ReservationTests
{
    //TODO: Refactor tests to use ValidReservation object?



    [Fact]
    public void Constructor_WithValidData_ShouldCreateReservation()
    {
        // Arrange
        Guid customerId = Guid.NewGuid();
        Guid roomId = Guid.NewGuid();
        DateOnly checkInDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1));
        DateOnly checkOutDate = DateOnly.FromDateTime(DateTime.Today.AddDays(5));
        decimal roomBasePrice = 100m;

        // Act
        Reservation reservation = new Reservation(customerId, roomId, checkInDate, checkOutDate, roomBasePrice);

        // Assert
        reservation.Should().NotBeNull();
        reservation.CustomerId.Should().Be(customerId);
        reservation.RoomId.Should().Be(roomId);
        reservation.CheckInDate.Should().Be(checkInDate);
        reservation.CheckOutDate.Should().Be(checkOutDate);
        reservation.TotalPrice.Should().Be(400m); // 4 nights * 100 per night
        reservation.Status.Should().Be(ReservationStatus.Active);
        reservation.CreatedAt
            .Should()
            .BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
    }

    [Fact]
    public void Constructor_WithInvalidDates_ShouldThrowArgumentException()
    {
        // Arrange
        Guid customerId = Guid.NewGuid();
        Guid roomId = Guid.NewGuid();
        DateOnly checkInDate = DateOnly.FromDateTime(DateTime.Today.AddDays(5));
        DateOnly checkOutDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1));
        decimal roomBasePrice = 100m;

        // Act
        Action act = () => new Reservation(customerId, roomId, checkInDate, checkOutDate, roomBasePrice);
        
        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Check-out date must be after check-in date*");
    }

    [Fact]
    public void Constructor_WithEmptyCustomerId_ShouldThrowArgumentException()
    {
        // Arrange
        Guid roomId = Guid.NewGuid();
        DateOnly checkInDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1));
        DateOnly checkOutDate = DateOnly.FromDateTime(DateTime.Today.AddDays(5));
        decimal roomBasePrice = 100m;
        
        // Act
        Action act = () => new Reservation(Guid.Empty, roomId, checkInDate, checkOutDate, roomBasePrice);
        
        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Customer ID is required*");
    }

    [Fact]
    public void Constructor_WithEmptyRoomId_ShouldThrowArgumentException()
    {
        // Arrange
        Guid customerId = Guid.NewGuid();
        DateOnly checkInDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1));
        DateOnly checkOutDate = DateOnly.FromDateTime(DateTime.Today.AddDays(5));
        decimal roomBasePrice = 100m;
        
        // Act
        Action act = () => new Reservation(customerId, Guid.Empty, checkInDate, checkOutDate, roomBasePrice);
        
        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Room ID is required*");
    }

    [Fact]
    public void Constructor_WithNegativeRoomBasePrice_ShouldThrowArgumentException()
    {
        // Arrange
        Guid customerId = Guid.NewGuid();
        Guid roomId = Guid.NewGuid();
        DateOnly checkInDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1));
        DateOnly checkOutDate = DateOnly.FromDateTime(DateTime.Today.AddDays(5));
        decimal roomBasePrice = -100m;
        
        // Act
        Action act = () => new Reservation(customerId, roomId, checkInDate, checkOutDate, roomBasePrice);
        
        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Total price cannot be negative*");
    }

    [Fact]
    public void Constructor_WithOneNight_ShouldCalculateCorrectly()
    {
        //Arrange
        Guid customerId = Guid.NewGuid();
        Guid roomId = Guid.NewGuid();
        DateOnly checkInDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1));
        DateOnly checkOutDate = DateOnly.FromDateTime(DateTime.Today.AddDays(2));
        decimal roomBasePrice = 100m;

        //Act
        Reservation reservation = new Reservation(customerId, roomId, checkInDate, checkOutDate, roomBasePrice);

        //Assert
        reservation.Should().NotBeNull();
        reservation.TotalPrice.Should().Be(100m); // 1 night * 100 per night

    }

    //Is this test necessary? It is already tested in the constructor test, but it is good to have a separate test for the price calculation logic as well.
    [Fact]
    public void CalculateTotalPrice_ShouldCalculateCorrectly()
    {
        // Arrange
        Guid customerId = Guid.NewGuid();
        Guid roomId = Guid.NewGuid();
        DateOnly checkInDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1));
        DateOnly checkOutDate = DateOnly.FromDateTime(DateTime.Today.AddDays(5));
        decimal roomBasePrice = 100m;

        // Act
        Reservation reservation = new Reservation(customerId, roomId, checkInDate, checkOutDate, roomBasePrice);

        // Assert
        reservation.TotalPrice.Should().Be(400m); // 4 nights * 100 per night
    }

    [Fact]
    public void CancelReservation_WhenActive_ShouldSetStatusToCancelled()
    {
        // Arrange
        Guid customerId = Guid.NewGuid();
        Guid roomId = Guid.NewGuid();
        DateOnly checkInDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1));
        DateOnly checkOutDate = DateOnly.FromDateTime(DateTime.Today.AddDays(5));
        decimal roomBasePrice = 100m;
       
        Reservation reservation = new Reservation(customerId, roomId, checkInDate, checkOutDate, roomBasePrice);
        
        // Act        
        reservation.CancelReservation();
        
        // Assert
        reservation.Status.Should().Be(ReservationStatus.Cancelled);
        reservation.UpdatedAt.Should().NotBeNull();
        reservation.UpdatedAt
            .Should()
            .BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));

    }

    [Fact]
    public void CancelReservation_WhenNotActive_ShouldThrowInvalidOperationException()
    {
        // Arrange
        Guid customerId = Guid.NewGuid();
        Guid roomId = Guid.NewGuid();
        DateOnly checkInDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1));
        DateOnly checkOutDate = DateOnly.FromDateTime(DateTime.Today.AddDays(5));
        decimal roomBasePrice = 100m;
       
        Reservation reservation = new Reservation(customerId, roomId, checkInDate, checkOutDate, roomBasePrice);
        reservation.CancelReservation(); // First cancel to set status to Cancelled
        
        // Act
        Action act = () => reservation.CancelReservation(); // Try to cancel again
        
        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*Reservation is not active and cannot be cancelled*");
    }

    [Fact]
    public void CompleteReservation_ShouldSetStatusToCompleted()
    {
        // Arrange
        Guid customerId = Guid.NewGuid();
        Guid roomId = Guid.NewGuid();
        DateOnly checkInDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-5));
        DateOnly checkOutDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-1));
        decimal roomBasePrice = 100m;
       
        Reservation reservation = new Reservation(customerId, roomId, checkInDate, checkOutDate, roomBasePrice);
        
        // Act        
        reservation.CompleteReservation();
        
        // Assert
        reservation.Status.Should().Be(ReservationStatus.Completed);
        reservation.UpdatedAt.Should().NotBeNull();
        reservation.UpdatedAt
            .Should()
            .BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
    }

    [Fact]
    public void CompleteReservation_WhenNotActive_ShouldThrowInvalidOperationException()
    {
        // Arrange
        Guid customerId = Guid.NewGuid();
        Guid roomId = Guid.NewGuid();
        DateOnly checkInDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-5));
        DateOnly checkOutDate = DateOnly.FromDateTime(DateTime.Today.AddDays(-1));
        decimal roomBasePrice = 100m;
       
        Reservation reservation = new Reservation(customerId, roomId, checkInDate, checkOutDate, roomBasePrice);
        reservation.CancelReservation(); // First cancel to set status to Cancelled
        
        // Act
        Action act = () => reservation.CompleteReservation(); // Try to complete a cancelled reservation
        
        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*Reservation is not active and cannot be completed*");
    }

    [Fact]
    public void UpdateReservation_WhenActive_ShouldUpdateDetails()
    {
        // Arrange
        Guid customerId = Guid.NewGuid();
        Guid roomId = Guid.NewGuid();
        DateOnly checkInDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1));
        DateOnly checkOutDate = DateOnly.FromDateTime(DateTime.Today.AddDays(5));
        decimal roomBasePrice = 100m;

        Reservation reservation = new Reservation(customerId, roomId, checkInDate, checkOutDate, roomBasePrice);
        var originalId = reservation.Id;

        // Act
        DateOnly newCheckInDate = DateOnly.FromDateTime(DateTime.Today.AddDays(2));
        DateOnly newCheckOutDate = DateOnly.FromDateTime(DateTime.Today.AddDays(6));
        decimal newRoomBasePrice = 120m;
        
        reservation.UpdateReservation(newCheckInDate, newCheckOutDate, newRoomBasePrice);
        
        // Assert
        reservation.CheckInDate.Should().Be(newCheckInDate);
        reservation.CheckOutDate.Should().Be(newCheckOutDate);
        reservation.TotalPrice.Should().Be(480m); // 4 nights * 120 per night
        reservation.Id.Should().Be(originalId);
        reservation.UpdatedAt.Should().NotBeNull();
        reservation.UpdatedAt
            .Should()
            .BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));

    }

    [Fact]
    public void UpdateReservation_WhenNotActive_ShouldThrowInvalidOperationException()
    {
        // Arrange
        Guid customerId = Guid.NewGuid();
        Guid roomId = Guid.NewGuid();
        DateOnly checkInDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1));
        DateOnly checkOutDate = DateOnly.FromDateTime(DateTime.Today.AddDays(5));
        decimal roomBasePrice = 100m;

        Reservation reservation = new Reservation(customerId, roomId, checkInDate, checkOutDate, roomBasePrice);
        reservation.CancelReservation(); // First cancel to set status to Cancelled

        // Act
        Action act = () => reservation.UpdateReservation(checkInDate,checkOutDate, roomBasePrice);

        //Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*Only active reservations can be updated*");
    }

    [Fact]
    public void UpdateReservation_WithInvalidDates_ShouldThrowArgumentException()
    {
        // Arrange
        Guid customerId = Guid.NewGuid();
        Guid roomId = Guid.NewGuid();
        DateOnly checkInDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1));
        DateOnly checkOutDate = DateOnly.FromDateTime(DateTime.Today.AddDays(5));
        decimal roomBasePrice = 100m;
        Reservation reservation = new Reservation(customerId, roomId, checkInDate, checkOutDate, roomBasePrice);
        
        // Act
        DateOnly newCheckInDate = DateOnly.FromDateTime(DateTime.Today.AddDays(5));
        DateOnly newCheckOutDate = DateOnly.FromDateTime(DateTime.Today.AddDays(1));
        
        Action act = () => reservation.UpdateReservation(newCheckInDate, newCheckOutDate, roomBasePrice);
        
        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Check-out date must be after check-in date*");
    }
}
