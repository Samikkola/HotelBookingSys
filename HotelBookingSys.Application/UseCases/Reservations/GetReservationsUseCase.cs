using HotelBookingSys.Application.Common.Result;
using HotelBookingSys.Application.DTOs.ReservationDtos;
using HotelBookingSys.Domain.Interfaces;
using HotelBookingSys.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBookingSys.Application.UseCases.Reservations;

public class GetReservationsUseCase
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IRoomRepository _roomRepository;

    public GetReservationsUseCase(IReservationRepository reservationRepository, IRoomRepository roomRepository)
    {
        _reservationRepository = reservationRepository;
        _roomRepository = roomRepository;
    }

    /// <summary>
    /// Retrieves reservations from the repository using optional filters.
    /// Maps the domain entities to ReservationResponseDto, including room numbers, and returns a Result containing the list of reservations.
    /// </summary>
    /// <param name="filter"></param>
    /// <returns></returns>
    public async Task<Result<IEnumerable<ReservationResponseDto>>> ExecuteAsync(ReservationFilterDto? filter = null)
    {
        if (filter is not null && filter.FromDate.HasValue && filter.ToDate.HasValue && filter.FromDate > filter.ToDate)
            return Result<IEnumerable<ReservationResponseDto>>.Failure(ErrorCode.Validation, "FromDate must be on or before ToDate.");

        var reservationsTask = _reservationRepository.GetAllAsync(
            filter?.CustomerId,
            filter?.RoomId,
            filter?.Status,
            filter?.FromDate,
            filter?.ToDate);
        var roomsTask = _roomRepository.GetAllAsync();
        // Wait for both tasks to complete
        await Task.WhenAll(reservationsTask, roomsTask);

        // Create a dictionary to quickly lookup room numbers by room ID
        var reservations = reservationsTask.Result;
        var rooms = roomsTask.Result.ToDictionary(r => r.Id, r => r.RoomNumber);

        // Map reservations to DTOs, including room numbers
        return Result<IEnumerable<ReservationResponseDto>>.Success(reservations.Select(r => MapToDto(r, rooms)));

    }

    private ReservationResponseDto MapToDto(
        Reservation reservation,
        IReadOnlyDictionary<Guid, int> rooms)
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
