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

    public async Task<ReservationResponseDto> ExecuteAsync(Guid reservationId, DateOnly newCheckIn, DateOnly newCheckOut)
    {
        var reservation = await _reservationRepository.GetByIdAsync(reservationId);
        if (reservation == null)
            throw new ArgumentException($"Reservation with ID {reservationId} not found.");

        var room = await _roomRepository.GetByIdAsync(reservation.RoomId);
        if (room == null)
            throw new InvalidOperationException("Associated room not found.");

        // Check overlapping (excluding the current reservation)
        var overlapping = await _reservationRepository.GetOverlappingReservationsByRoomIdAsync(reservation.RoomId, newCheckIn, newCheckOut);
        if (overlapping.Any(r => r.Id != reservation.Id))
            throw new InvalidOperationException("Room is not available for the new dates.");

        // Update domain
        reservation.UpdateReservation(newCheckIn, newCheckOut, room.BasePrice);
        
        await _reservationRepository.UpdateAsync(reservation);

        return new ReservationResponseDto
        {
            Id = reservation.Id,
            CustomerId = reservation.CustomerId,
            RoomId = reservation.RoomId,
            RoomNumber = room.RoomNumber,
            CheckInDate = reservation.CheckInDate,
            CheckOutDate = reservation.CheckOutDate,
            TotalPrice = reservation.TotalPrice,
            Status = reservation.Status.ToString()
        };
    }
}
