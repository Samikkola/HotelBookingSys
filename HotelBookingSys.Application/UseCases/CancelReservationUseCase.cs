using HotelBookingSys.Application.DTOs;
using HotelBookingSys.Application.Interfaces;
using System;
using System.Threading.Tasks;

namespace HotelBookingSys.Application.UseCases;

public class CancelReservationUseCase
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IRoomRepository roomRepository;

    public CancelReservationUseCase(IReservationRepository reservationRepository, IRoomRepository roomRepository)
    {
        _reservationRepository = reservationRepository;
        this.roomRepository = roomRepository;
    }

    public async Task<ReservationResponseDto> ExecuteAsync(Guid reservationId)
    {
        var reservation = await _reservationRepository.GetByIdAsync(reservationId);
        
        if (reservation == null)
            throw new ArgumentException($"Reservation with ID {reservationId} not found.");

        //Get the roomnumber for the response dto
        var room = await roomRepository.GetByIdAsync(reservation.RoomId);
        if (room == null)
            throw new InvalidOperationException("Associated room not found.");

        //Soft delete by changing status to Cancelled
        reservation.CancelReservation();
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
