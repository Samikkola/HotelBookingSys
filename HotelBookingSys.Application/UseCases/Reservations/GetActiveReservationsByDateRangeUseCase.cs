using HotelBookingSys.Application.Common.Result;
using HotelBookingSys.Application.DTOs.ReservationDtos;
using HotelBookingSys.Application.Mappings.Reservations;
using HotelBookingSys.Domain.Interfaces;

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

        var reservations = await _reservationRepository.GetActiveReservationsByDateRangeAsync(from, to);
        var rooms = await _roomRepository.GetAllAsync();
        var roomsById = rooms.ToDictionary(r => r.Id, r => r.RoomNumber);

        // Map reservations to DTOs, including room numbers
        return Result<IEnumerable<ReservationResponseDto>>.Success(
            reservations.Select(r => ReservationMapper.ToResponseDto(r, roomsById)));
    }
}


