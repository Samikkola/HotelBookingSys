using HotelBookingSys.Application.Common.Result;
using HotelBookingSys.Application.DTOs;
using HotelBookingSys.Application.Interfaces;
using System;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBookingSys.Application.UseCases;

public class UpdateReservationDatesUseCase
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IRoomRepository _roomRepository;

    public UpdateReservationDatesUseCase(IReservationRepository reservationRepository, IRoomRepository roomRepository)
    {
        _reservationRepository = reservationRepository;
        _roomRepository = roomRepository;
    }

    public async Task<Result<ReservationResponseDto>> ExecuteAsync(Guid reservationId, DateOnly newCheckIn, DateOnly newCheckOut)
    {
        var reservation = await _reservationRepository.GetByIdAsync(reservationId);
        if (reservation == null)
            return Result<ReservationResponseDto>.Failure(ErrorCode.NotFound, $"Reservation with ID {reservationId} not found.");

        var room = await _roomRepository.GetByIdAsync(reservation.RoomId);
        if (room == null)
            return Result<ReservationResponseDto>.Failure(ErrorCode.NotFound, "Associated room not found.");

        // Check overlapping (excluding the current reservation)
        var overlapping = await _reservationRepository.GetOverlappingReservationsByRoomIdAsync(reservation.RoomId, newCheckIn, newCheckOut);
        if (overlapping.Any(r => r.Id != reservation.Id))
            return Result<ReservationResponseDto>.Failure(ErrorCode.Conflict, "Room is not available for the new dates.");

        //TODO: Add date validation -> only future dates accepted

        try
        {   // Update domain
            reservation.UpdateReservation(newCheckIn, newCheckOut, room.BasePrice);
        }
        catch (ArgumentException ex)//Catch for domain exceptions
        {
            return Result<ReservationResponseDto>.Failure(ErrorCode.Validation, ex.Message);
        }
        catch (InvalidOperationException ex)//Catch for domain exceptions
        {
            return Result<ReservationResponseDto>.Failure(ErrorCode.Conflict, ex.Message);
        }
        
        await _reservationRepository.UpdateAsync(reservation);

        return Result<ReservationResponseDto>.Success(new ReservationResponseDto
        {
            Id = reservation.Id,
            CustomerId = reservation.CustomerId,
            RoomId = reservation.RoomId,
            RoomNumber = room.RoomNumber,
            CheckInDate = reservation.CheckInDate,
            CheckOutDate = reservation.CheckOutDate,
            NumberOfGuests = reservation.NumberOfGuests,
            TotalPrice = reservation.TotalPrice,
            Status = reservation.Status.ToString()
        });
    }
}
