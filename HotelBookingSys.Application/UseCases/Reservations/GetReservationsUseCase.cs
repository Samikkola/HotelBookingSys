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

        // Retrieve reservations based on filters
        var reservations = await _reservationRepository.GetAllAsync(
            filter?.CustomerId,
            filter?.RoomId,
            filter?.Status,
            filter?.FromDate,
            filter?.ToDate);
        // Retrieve all rooms to map room numbers
        var rooms = await _roomRepository.GetAllAsync();
        // Create a dictionary to map room IDs to room numbers for efficient lookup
        var roomMap = rooms.ToDictionary(r => r.Id, r => r.RoomNumber);
        
        // Map reservations to DTOs, including room numbers
        return Result<IEnumerable<ReservationResponseDto>>.Success(reservations.Select(r => MapToDto(r, roomMap)));
    }

    private static ReservationResponseDto MapToDto(
        Reservation reservation,
        Dictionary<Guid, int> rooms)
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
            Status = reservation.Status.ToString(),
            CreatedAt = reservation.CreatedAt,
            UpdatedAt = reservation.UpdatedAt
        };
    }

}
