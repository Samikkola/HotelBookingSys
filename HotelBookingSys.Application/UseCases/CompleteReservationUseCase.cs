using HotelBookingSys.Application.Common.Result;
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

    public async Task<Result<ReservationResponseDto>> ExecuteAsync(Guid reservationId)
    {
        var reservation = await _reservationRepository.GetByIdAsync(reservationId);
        
        if (reservation == null)
            return Result<ReservationResponseDto>.Failure(ErrorCode.NotFound, $"Reservation with ID {reservationId} not found.");

        //Get roomnumber for the response dto
        var room = await _roomRepository.GetByIdAsync(reservation.RoomId);
        if (room == null)
            return Result<ReservationResponseDto>.Failure(ErrorCode.NotFound, "Associated room not found.");

        try
        {
            reservation.CompleteReservation();
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
            TotalPrice = reservation.TotalPrice,
            Status = reservation.Status.ToString()
        });
    }
}
