using HotelBookingSys.Application.Common.Result;
using HotelBookingSys.Application.DTOs.ReservationDtos;
using HotelBookingSys.Domain.Interfaces;

namespace HotelBookingSys.Application.UseCases.Reservations;

public class UpdateReservationUseCase
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IRoomRepository _roomRepository;

    public UpdateReservationUseCase(IReservationRepository reservationRepository, IRoomRepository roomRepository)
    {
        _reservationRepository = reservationRepository;
        _roomRepository = roomRepository;
    }

    /// <summary>
    /// Updates room, dates and guest count of an existing reservation.
    /// Validates availability and occupancy rules and returns a Result containing either the updated ReservationResponseDto or error information if the operation fails.
    /// </summary>
    /// <param name="reservationId"></param>
    /// <param name="dto"></param>
    /// <returns></returns>
    public async Task<Result<ReservationResponseDto>> ExecuteAsync(Guid reservationId, UpdateReservationDto dto)
    {
        var reservation = await _reservationRepository.GetByIdAsync(reservationId);
        if (reservation == null)
            return Result<ReservationResponseDto>.Failure(ErrorCode.NotFound, $"Reservation with ID {reservationId} not found.");

        var targetRoomId = dto.RoomId ?? reservation.RoomId;
        var targetCheckIn = dto.NewCheckInDate ?? reservation.CheckInDate;
        var targetCheckOut = dto.NewCheckOutDate ?? reservation.CheckOutDate;
        var targetGuestCount = dto.GuestCount ?? reservation.NumberOfGuests;

        var room = await _roomRepository.GetByIdAsync(targetRoomId);
        if (room == null)
            return Result<ReservationResponseDto>.Failure(ErrorCode.NotFound, "Associated room not found.");

        if (targetGuestCount > room.RoomCapacity)
            return Result<ReservationResponseDto>.Failure(ErrorCode.Validation, $"Number of guests exceeds room capacity of {room.RoomCapacity}.");

        var overlapping = await _reservationRepository.GetOverlappingReservationsByRoomIdAsync(targetRoomId, targetCheckIn, targetCheckOut);
        if (overlapping.Any(r => r.Id != reservation.Id))
            return Result<ReservationResponseDto>.Failure(ErrorCode.Conflict, "Room is not available for the new dates.");

        try
        {
            reservation.UpdateReservationDetails(
                targetRoomId,
                targetCheckIn,
                targetCheckOut,
                targetGuestCount,
                room.RoomCapacity,
                room.BasePrice);
        }
        catch (ArgumentException ex)
        {
            return Result<ReservationResponseDto>.Failure(ErrorCode.Validation, ex.Message);
        }
        catch (InvalidOperationException ex)
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
