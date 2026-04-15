using HotelBookingSys.Domain.Enums;

namespace HotelBookingSys.Domain.Entities;

public class Reservation : BaseEntity
{
    // Foreign keys
    public Guid CustomerId { get; private set; }
    public Guid RoomId { get; private set; }

    public DateOnly CheckInDate { get; private set; } 
    public DateOnly CheckOutDate { get; private set; } 

    public int NumberOfGuests { get; private set; }

    public decimal TotalPrice { get; private set; }
    public ReservationStatus Status { get; private set; }

    //Navigation properties TODO: Decide if to use these or not
    // public Customer Customer { get; private set; }
    // public Room Room { get; private set; }

    //Parametrelles constructor for EF Core
    private Reservation() 
    { 
              
    }

    //Constructor
    public Reservation(Guid customerId, Guid roomId, DateOnly checkInDate, DateOnly checkOutDate, int numberOfGuests, int roomCapacity, decimal roomBasePrice)
    {
        // Domain validation
        if (customerId == Guid.Empty)
            throw new ArgumentException("Customer ID is required.", nameof(customerId));

        if (roomId == Guid.Empty)
            throw new ArgumentException("Room ID is required.", nameof(roomId));

        if (checkInDate >= checkOutDate)
            throw new ArgumentException("Check-out date must be after check-in date.");

        if (numberOfGuests <= 0)
            throw new ArgumentException("Number of guests must be greater than zero.", nameof(numberOfGuests));

        if (numberOfGuests > roomCapacity)
            throw new ArgumentException($"Number of guests exceeds room capacity of {roomCapacity}.", nameof(numberOfGuests));

        if (roomBasePrice < 0)
            throw new ArgumentException("Total price cannot be negative.", nameof(roomBasePrice));

        CustomerId = customerId;
        RoomId = roomId;
        CheckInDate = checkInDate;
        CheckOutDate = checkOutDate;
        NumberOfGuests = numberOfGuests;       
        CalculateTotalPrice(roomBasePrice); // Calculate TotalPrice based on room base price and reservation duration
        Status = ReservationStatus.Active; 
        
    }

    

    /// <summary>
    /// Calculates total price of the reservation and adds seasonal pricing
    /// </summary>
    /// <param name="roomBasePrice"></param>
    /// <exception cref="InvalidOperationException"></exception>
    private void CalculateTotalPrice(decimal roomBasePrice)
    {
        int nights = CheckOutDate.DayNumber - CheckInDate.DayNumber;
        if (nights <= 0)
            throw new InvalidOperationException("Reservation period must be at least 1 night");

        decimal total = 0m;
        var currentDate = CheckInDate;

        // Loop through each night of the reservation to apply seasonal pricing
        for (int i = 0; i < nights; i++)
        {
            decimal nightPrice = roomBasePrice;

            //Dates for seasonal pricing
            bool isSummer = currentDate.Month >= 6 && currentDate.Month <= 8;
            bool isChristmas = (currentDate.Month == 12 && currentDate.Day >= 20) ||
                               (currentDate.Month == 1 && currentDate.Day <= 6);

            if (isSummer || isChristmas)
            {
                nightPrice *= 1.3m;
            }

            total += nightPrice;
            currentDate = currentDate.AddDays(1);
        }

        TotalPrice = total;
    }

    public void CancelReservation()
    {
        if (Status != ReservationStatus.Active)
            throw new InvalidOperationException("Reservation is not active and cannot be cancelled.");

        Status = ReservationStatus.Cancelled;
        SetUpdatedAt();
    }

    public void CompleteReservation()
        {
            if (Status != ReservationStatus.Active)
                throw new InvalidOperationException("Reservation is not active and cannot be completed.");

        Status = ReservationStatus.Completed;
        SetUpdatedAt();
    }

    /// <summary>
    /// Updates reservation room, dates and guest count.
    /// </summary>
    /// <param name="roomId"></param>
    /// <param name="newCheckInDate"></param>
    /// <param name="newCheckOutDate"></param>
    /// <param name="numberOfGuests"></param>
    /// <param name="roomCapacity"></param>
    /// <param name="roomBasePrice"></param>
    public void UpdateReservationDetails(
        Guid roomId,
        DateOnly newCheckInDate,
        DateOnly newCheckOutDate,
        int numberOfGuests,
        int roomCapacity,
        decimal roomBasePrice)
    {
        if (Status != ReservationStatus.Active)
            throw new InvalidOperationException("Only active reservations can be updated.");

        if (roomId == Guid.Empty)
            throw new ArgumentException("Room ID is required.", nameof(roomId));

        if (newCheckInDate >= newCheckOutDate)
            throw new ArgumentException("Check-out date must be after check-in date.");

        if (numberOfGuests <= 0)
            throw new ArgumentException("Number of guests must be greater than zero.", nameof(numberOfGuests));

        if (numberOfGuests > roomCapacity)
            throw new ArgumentException($"Number of guests exceeds room capacity of {roomCapacity}.", nameof(numberOfGuests));

        if (roomBasePrice < 0)
            throw new ArgumentException("Total price cannot be negative.", nameof(roomBasePrice));

        RoomId = roomId;
        CheckInDate = newCheckInDate;
        CheckOutDate = newCheckOutDate;
        NumberOfGuests = numberOfGuests;
        CalculateTotalPrice(roomBasePrice);
        SetUpdatedAt();
    }

    
    //TODO: Max occupancy validation?
}
