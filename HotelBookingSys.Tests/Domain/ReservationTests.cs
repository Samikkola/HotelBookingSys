using FluentAssertions;
using HotelBookingSys.Domain.Entities;
using HotelBookingSys.Domain.Enums;

namespace HotelBookingSys.Tests.Domain;

public class ReservationTests
{
    private static readonly Guid CustomerId = Guid.NewGuid();
    private static readonly Guid RoomId = Guid.NewGuid();
    private static readonly DateOnly BaseCheckIn = new(2026, 2, 10);
    private static readonly DateOnly BaseCheckOut = new(2026, 2, 14);
    private const decimal BasePrice = 100m;

    private Reservation ValidReservation => new(CustomerId, RoomId, BaseCheckIn, BaseCheckOut, BasePrice);

    [Fact]
    public void Constructor_WithValidData_ShouldCreateReservation()
    {
        // Arrange
        var reservation = ValidReservation;

        // Assert
        reservation.Should().NotBeNull();
        reservation.CustomerId.Should().Be(CustomerId);
        reservation.RoomId.Should().Be(RoomId);
        reservation.CheckInDate.Should().Be(BaseCheckIn);
        reservation.CheckOutDate.Should().Be(BaseCheckOut);
        reservation.TotalPrice.Should().Be(400m); // 4 nights * 100 per night
        reservation.Status.Should().Be(ReservationStatus.Active);
        reservation.CreatedAt.Should().BeCloseTo(DateTime.UtcNow, TimeSpan.FromSeconds(2));
    }

    [Fact]
    public void Constructor_WithInvalidDates_ShouldThrowArgumentException()
    {
        // Arrange
        // Swap the dates to create a invalid scenario
        DateOnly checkInDate = BaseCheckOut;
        DateOnly checkOutDate = BaseCheckIn;

        // Act
        Action act = () => new Reservation(CustomerId, RoomId, checkInDate, checkOutDate, BasePrice);
        
        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Check-out date must be after check-in date*");
    }

    [Fact]
    public void Constructor_WithEmptyCustomerId_ShouldThrowArgumentException()
    {
       
        // Act
        Action act = () => new Reservation(Guid.Empty, RoomId, BaseCheckIn, BaseCheckOut, BasePrice);
        
        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Customer ID is required*");
    }

    [Fact]
    public void Constructor_WithEmptyRoomId_ShouldThrowArgumentException()
    {
              
        // Act
        Action act = () => new Reservation(CustomerId, Guid.Empty, BaseCheckIn, BaseCheckOut, BasePrice);
        
        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Room ID is required*");
    }

    [Fact]
    public void Constructor_WithNegativeRoomBasePrice_ShouldThrowArgumentException()
    {
             
        // Act
        Action act = () => new Reservation(CustomerId, RoomId, BaseCheckIn, BaseCheckOut, -100m);
        
        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Total price cannot be negative*");
    }

    [Fact]
    public void Constructor_WithOneNight_ShouldCalculateCorrectly()
    {
        //Arrange    
        DateOnly checkOutDate = BaseCheckIn.AddDays(1);

        //Act
        Reservation reservation = new Reservation(CustomerId, RoomId, BaseCheckIn, checkOutDate, BasePrice);

        //Assert
        reservation.Should().NotBeNull();
        reservation.TotalPrice.Should().Be(100m); // 1 night * 100 per night

    }

    [Fact] // Maybe futile test? 
    public void CalculateTotalPrice_ShouldCalculateCorrectly()
    {
        // Arrange
        Reservation reservation = ValidReservation;
      
        // Assert
        reservation.TotalPrice.Should().Be(400m); // 4 nights * 100 per night
    }

    [Fact]
    public void CancelReservation_WhenActive_ShouldSetStatusToCancelled()
    {
        // Arrange
        Reservation reservation = ValidReservation;
        
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
        Reservation reservation = ValidReservation;
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
        Reservation reservation = ValidReservation;
        
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
        Reservation reservation = ValidReservation;
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
        Reservation reservation = ValidReservation;
        var originalId = reservation.Id;

        // Act
        DateOnly newCheckInDate = BaseCheckIn.AddDays(1);
        DateOnly newCheckOutDate = BaseCheckOut.AddDays(1);
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
        Reservation reservation = ValidReservation;
        reservation.CancelReservation(); // First cancel to set status to Cancelled

        // Act
        Action act = () => reservation.UpdateReservation(BaseCheckIn, BaseCheckOut, BasePrice);

        //Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage("*Only active reservations can be updated*");
    }

    [Fact]
    public void UpdateReservation_WithInvalidDates_ShouldThrowArgumentException()
    {
        // Arrange
        Reservation reservation = ValidReservation;
        
        // Act
        // Swap dates to get invalid scenario
        DateOnly newCheckInDate = BaseCheckOut;
        DateOnly newCheckOutDate = BaseCheckIn;
        
        Action act = () => reservation.UpdateReservation(newCheckInDate, newCheckOutDate, BasePrice);
        
        // Assert
        act.Should().Throw<ArgumentException>()
            .WithMessage("*Check-out date must be after check-in date*");
    }

    [Fact]
    public void CalculateTotalPrice_ShouldApplySummerPricing()
    {
        var checkInDate = new DateOnly(2026, 6, 1);
        var checkOutDate = new DateOnly(2026, 6, 3);
        var reservation = new Reservation(CustomerId, RoomId, checkInDate, checkOutDate, BasePrice);

        reservation.TotalPrice.Should().Be(260m); // 2 nights * 100 * 1.3
    }

    [Fact]
    public void CalculateTotalPrice_ShouldApplyChristmasPricing()
    {
        var checkInDate = new DateOnly(2026, 12, 20);
        var checkOutDate = new DateOnly(2026, 12, 22);
        var reservation = new Reservation(CustomerId, RoomId, checkInDate, checkOutDate, BasePrice);

        reservation.TotalPrice.Should().Be(260m); // 2 nights * 100 * 1.3
    }

    [Fact]
    public void CalculateTotalPrice_ShouldApplyMixedSeasonPricing()
    {
        var checkInDate = new DateOnly(2026, 8, 31);
        var checkOutDate = new DateOnly(2026, 9, 2);
        var reservation = new Reservation(CustomerId, RoomId, checkInDate, checkOutDate, BasePrice);

        reservation.TotalPrice.Should().Be(230m); // 1 seasonal + 1 normal
    }

    [Fact]
    public void CalculateTotalPrice_ShouldPreserveDecimalPrecision()
    {
        var checkInDate = new DateOnly(2026, 6, 1);
        var checkOutDate = new DateOnly(2026, 6, 2);
        var reservation = new Reservation(CustomerId, RoomId, checkInDate, checkOutDate, 99.99m);

        reservation.TotalPrice.Should().Be(129.987m);
    }
}
