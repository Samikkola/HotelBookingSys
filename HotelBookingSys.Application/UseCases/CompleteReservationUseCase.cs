using HotelBookingSys.Application.DTOs;
using HotelBookingSys.Application.Interfaces;
using System;
using System.Threading.Tasks;

namespace HotelBookingSys.Application.UseCases;

public class CompleteReservationUseCase
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IRoomRepository _roomRepository;

    public CompleteReservationUseCase(IReservationRepository reservationRepository, IRoomRepository roomRepository)
    {
        _reservationRepository = reservationRepository;
        _roomRepository = roomRepository;
    }

    public async Task<ReservationResponseDto> ExecuteAsync(Guid reservationId)
    {
        var reservation = await _reservationRepository.GetByIdAsync(reservationId);
        
        if (reservation == null)
            throw new ArgumentException($"Reservation with ID {reservationId} not found.");

        //Get roomnumber for the response dto
        var room = await _roomRepository.GetByIdAsync(reservation.RoomId);
        if (room == null)
            throw new InvalidOperationException("Associated room not found.");

        reservation.CompleteReservation();
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
