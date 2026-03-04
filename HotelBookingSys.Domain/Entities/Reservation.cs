using HotelBookingSys.Domain.Enums;

namespace HotelBookingSys.Domain.Entities;

public class Reservation
{
    public Guid Id { get; private set; }

    public Guid CustomerId { get; private set; }
    public int RoomNumber { get; private set; }

    public DateOnly CheckInDate { get; private set; } 
    public DateOnly CheckOutDate { get; private set; } 

    public decimal TotalPrice { get; private set; }
    public ReservationStatus Status { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime? UpdatedAt { get; private set; }

    //Constructor
    public Reservation(Guid customerId, int roomNumber, DateOnly checkInDate, DateOnly checkOutDate, decimal roomBasePrice)
    {
        // Domain validation
        if (customerId == Guid.Empty)
            throw new ArgumentException("Customer ID is required.", nameof(customerId));

        if (roomNumber <= 0)
            throw new ArgumentException("Room number is required.", nameof(roomNumber));

        if (checkInDate >= checkOutDate)
            throw new ArgumentException("Check-out date must be after check-in date.");

        if (roomBasePrice < 0)
            throw new ArgumentException("Total price cannot be negative.", nameof(roomBasePrice));

        Id = Guid.NewGuid();
        CustomerId = customerId;
        RoomNumber = roomNumber;
        CheckInDate = checkInDate;
        CheckOutDate = checkOutDate;
        CalculateTotalPrice(roomBasePrice); // Calculate TotalPrice based on room base price and reservation duration
        Status = ReservationStatus.Active; 
        CreatedAt = DateTime.UtcNow;
       
    }

    private void CalculateTotalPrice(decimal roomBasePrice)
    {
        // Use DateOnly.DayNumber to compute the difference in days since direct subtraction of DateOnly is not supported.
        int nights = CheckOutDate.DayNumber - CheckInDate.DayNumber;
        if (nights <= 0)
            throw new InvalidOperationException("Reservation period must be at least 1 night");

        TotalPrice = nights * roomBasePrice;
    }

    public void CancelReservation()
    {
        if (Status != ReservationStatus.Active)
            
            throw new InvalidOperationException("Reservation is not active and cannot be cancelled.");
        UpdatedAt = DateTime.UtcNow;
        Status = ReservationStatus.Cancelled;
    }
    
    public void CompleteReservation()
        {
            if (Status != ReservationStatus.Active)
                throw new InvalidOperationException("Reservation is not active and cannot be completed.");

        UpdatedAt = DateTime.UtcNow;
        Status = ReservationStatus.Completed;
    }
    public void UpdateReservation(DateOnly newCheckInDate, DateOnly newCheckOutDate, decimal roomBasePrice)
    {
        if (Status != ReservationStatus.Active)
            throw new InvalidOperationException("Only active reservations can be updated.");
        
        if (newCheckInDate >= newCheckOutDate)
            throw new ArgumentException("Check-out date must be after check-in date.");
        
        CheckInDate = newCheckInDate;
        CheckOutDate = newCheckOutDate;
        UpdatedAt = DateTime.UtcNow;
        CalculateTotalPrice(roomBasePrice); // Recalculate total price based on new dates
    }

    //TODO: Seasonal calculation?
}
