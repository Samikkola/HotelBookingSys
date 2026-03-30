using HotelBookingSys.Application.Common.Result;
using HotelBookingSys.Application.DTOs.ReservationDtos;
using HotelBookingSys.Application.Interfaces;
using HotelBookingSys.Domain.Entities;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBookingSys.Application.UseCases.Reservations;

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

    /// <summary>
    /// Creates a new reservation based on the provided DTO.
    /// Validates the input, checks for room availability, and returns a Result containing either the created ReservationResponseDto or error information if the operation fails.
    /// </summary>
    /// <param name="dto"></param>
    /// <returns></returns>
    public async Task<Result<ReservationResponseDto>> ExecuteAsync(CreateReservationDto dto)
    {
        
        // Validate customer and room existence
        var customer = await _customerRepository.GetByIdAsync(dto.CustomerId);
        if (customer == null)
            return Result<ReservationResponseDto>.Failure(ErrorCode.NotFound, "Customer not found.");

        //Gets the room details for the reservation creation
        var room = await _roomRepository.GetByRoomNumberAsync(dto.RoomNumber);
        if (room == null)
            return Result<ReservationResponseDto>.Failure(ErrorCode.NotFound, "Room not found.");
     
        // Check room availability (overlap)
        var overlappingReservations = await _reservationRepository
            .GetOverlappingReservationsByRoomIdAsync(room.Id, dto.CheckInDate, dto.CheckOutDate)
            ?? Array.Empty<Reservation>();
        if (overlappingReservations.Any())
            return Result<ReservationResponseDto>.Failure(ErrorCode.Conflict, "Room is already booked for the selected dates.");

        //TODO: Add date validation -> only future dates accepted

        //Create reservation with correct RoomId and Baseprice
        Reservation reservation;
        try
        {
            reservation = MapToDomain(dto, room.Id, room.RoomCapacity, room.BasePrice);
        }
        catch (ArgumentException ex)//Catch for domain exceptions
        {
            return Result<ReservationResponseDto>.Failure(ErrorCode.Validation, ex.Message);
        }
        catch (InvalidOperationException ex)//Catch for domain exceptions
        {
            return Result<ReservationResponseDto>.Failure(ErrorCode.Validation, ex.Message);
        }
        // Save reservation
        await _reservationRepository.AddAsync(reservation);

        //Map to DTO and return
        return Result<ReservationResponseDto>.Success(MapToDto(reservation, room.RoomNumber));
    }

    private Reservation MapToDomain(CreateReservationDto dto , Guid roomId, int roomCapacity, decimal basePrice)
    {
        return new Reservation(dto.CustomerId, roomId , dto.CheckInDate, dto.CheckOutDate, dto.NumberOfGuests, roomCapacity, basePrice); // TotalPrice will be set in domain
    }

    private static ReservationResponseDto MapToDto(Reservation reservation, int roomNumber)
    {
        return new ReservationResponseDto
        {
            Id = reservation.Id,
            CustomerId = reservation.CustomerId,
            RoomId = reservation.RoomId,
            RoomNumber = roomNumber,
            CheckInDate = reservation.CheckInDate,
            CheckOutDate = reservation.CheckOutDate,
            NumberOfGuests = reservation.NumberOfGuests,
            TotalPrice = reservation.TotalPrice,
            Status = reservation.Status.ToString() ?? string.Empty
        };
    }
}