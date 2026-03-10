

using HotelBookingSys.Application.Interfaces;
using HotelBookingSys.Domain.Entities;

namespace HotelBookingSys.Application.UseCases;

/// <summary>
/// Handles the creation of a reservation for a specified customer and room within a given date range.
/// </summary>
public class CreateReservationUseCase
{
    private readonly ICustomerRepository _customerRepository;
    private readonly IRoomRepository _roomRepository;
    private readonly IReservationRepository _reservationRepository;

    public CreateReservationUseCase(ICustomerRepository customerRepository, IRoomRepository roomRepository, IReservationRepository reservationRepository)
    {
        _customerRepository = customerRepository;
        _roomRepository = roomRepository;
        _reservationRepository = reservationRepository;
    }


    public async Task<Reservation> ExecuteAsync(Guid customerId, int roomNumber, DateOnly checkInDate, DateOnly checkOutDate)
    {
        // Validate customer and room existence
        var customer = await _customerRepository.GetByIdAsync(customerId);
        if (customer == null)
            throw new ArgumentException("Customer not found.", nameof(customerId));

        //Maps roomNumber (int) to roomId (Guid)
        var room = await _roomRepository.GetByRoomNumberAsync(roomNumber);
        if (room == null)
            throw new ArgumentException("Room not found.", nameof(roomNumber));

        //TODO: Implement in InMemeoryReservationRepository

        // Check room availability (overlap)
        //var overlappingReservations = await _reservationRepository
        //    .GetOverlappingReservationsAsync(room.Id, checkInDate, checkOutDate);
        //if (overlappingReservations.Any())
        //    throw new InvalidOperationException("Room is already booked for the selected dates.");

        // Create reservation
        var reservation = new Reservation(customerId, room.Id, checkInDate, checkOutDate, room.BasePrice);
        // Save reservation
        await _reservationRepository.AddAsync(reservation);
        return reservation;
    }
}