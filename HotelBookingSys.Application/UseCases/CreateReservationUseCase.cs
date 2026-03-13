

using HotelBookingSys.Application.DTOs;
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


    public async Task<ReservationResponseDto> ExecuteAsync(CreateReservationDto dto)
    {
        
        // Validate customer and room existence
        var customer = await _customerRepository.GetByIdAsync(dto.CustomerId);
        if (customer == null)
            throw new ArgumentException("Customer not found.", nameof(dto.CustomerId));

        //Maps roomNumber (int) to roomId (Guid)
        var room = await _roomRepository.GetByRoomNumberAsync(dto.RoomNumber);
        if (room == null)
            throw new ArgumentException("Room not found.", nameof(dto.RoomNumber));

        // Check room availability (overlap)
        var overlappingReservations = await _reservationRepository
            .GetOverlappingReservationsByRoomIdAsync(room.Id, dto.CheckInDate, dto.CheckOutDate);
        if (overlappingReservations.Any())
            throw new InvalidOperationException("Room is already booked for the selected dates.");

        //Create resvervation with correct RoomId and Baseprice
        var reservation = MapToDomain(dto, room.Id, room.BasePrice);
        // Save reservation
        await _reservationRepository.AddAsync(reservation);

        //Map to DTO and return
        return MapToDto(reservation);
    }

    private Reservation MapToDomain(CreateReservationDto dto , Guid roomId, decimal basePrice)
    {
        return new Reservation(dto.CustomerId, roomId , dto.CheckInDate, dto.CheckOutDate, basePrice); // TotalPrice will be set in domain
    }

    private ReservationResponseDto MapToDto(Reservation reservation)
    {
        //Gets teh room to get RoomNumber
        var room = _roomRepository.GetByIdAsync(reservation.RoomId).Result;

        return new ReservationResponseDto
        {
            Id = reservation.Id,
            CustomerId = reservation.CustomerId,
            RoomId = reservation.RoomId,
            RoomNumber = room?.RoomNumber ?? 0, // Fallback to 0 if room is not found
            CheckInDate = reservation.CheckInDate,
            CheckOutDate = reservation.CheckOutDate,
            TotalPrice = reservation.TotalPrice,
            Status = reservation.Status.ToString() ?? string.Empty
        };
    }
}