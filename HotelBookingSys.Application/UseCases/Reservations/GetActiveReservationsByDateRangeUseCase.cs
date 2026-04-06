using HotelBookingSys.Application.Common.Result;
using HotelBookingSys.Application.DTOs.ReservationDtos;
using HotelBookingSys.Domain.Interfaces;
using HotelBookingSys.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBookingSys.Application.UseCases.Reservations;

public class GetActiveReservationsByDateRangeUseCase
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IRoomRepository _roomRepository;

    public GetActiveReservationsByDateRangeUseCase(IReservationRepository reservationRepository, IRoomRepository roomRepository)
    {
        _reservationRepository = reservationRepository;
        _roomRepository = roomRepository;
    }

    /// <summary>
    /// Retrieves all active reservations that fall within the specified date range.
    /// Validates the input dates and returns a Result containing either the list of ReservationResponseDto or error information if the operation fails.
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns></returns>
    public async Task<Result<IEnumerable<ReservationResponseDto>>> ExecuteAsync(DateOnly from, DateOnly to)
    {
        if (from > to)
            return Result<IEnumerable<ReservationResponseDto>>.Failure(ErrorCode.Validation, "From date must be on or before To date.");

        var reservationsTask = _reservationRepository.GetActiveReservationsByDateRangeAsync(from, to);
        var roomsTask = _roomRepository.GetAllAsync();

        await Task.WhenAll(reservationsTask, roomsTask);

        var reservations = reservationsTask.Result;
        var rooms = roomsTask.Result.ToDictionary(r => r.Id, r => r.RoomNumber);

        return Result<IEnumerable<ReservationResponseDto>>.Success(reservations.Select(r => MapToDto(r, rooms)));
    }

    private static ReservationResponseDto MapToDto(Reservation reservation, IReadOnlyDictionary<Guid, int> rooms)
    {
        return new ReservationResponseDto
        {
            Id = reservation.Id,
            CustomerId = reservation.CustomerId,
            RoomId = reservation.RoomId,
            RoomNumber = rooms.TryGetValue(reservation.RoomId, out var roomNum) ? roomNum : 0,
            CheckInDate = reservation.CheckInDate,
            CheckOutDate = reservation.CheckOutDate,
            NumberOfGuests = reservation.NumberOfGuests,
            TotalPrice = reservation.TotalPrice,
            Status = reservation.Status.ToString()
        };
    }
}
