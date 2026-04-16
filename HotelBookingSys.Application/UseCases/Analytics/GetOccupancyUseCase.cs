using HotelBookingSys.Application.DTOs;
using HotelBookingSys.Application.Common.Result;
using HotelBookingSys.Domain.Interfaces;

namespace HotelBookingSys.Application.UseCases.Analytics;

public class GetOccupancyUseCase
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IRoomRepository _roomRepository;

    public GetOccupancyUseCase(IReservationRepository reservationRepository, IRoomRepository roomRepository)
    {
        _reservationRepository = reservationRepository;
        _roomRepository = roomRepository;
    }

    /// <summary>
    /// Returns occupancy metrics for the given date range.
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns></returns>
    public async Task<Result<OccupancyDto>> ExecuteAsync(DateOnly from, DateOnly to)
    {
        if (from > to)
            return Result<OccupancyDto>.Failure(ErrorCode.Validation, "From date must be on or before To date.");

        var roomTotal = await _roomRepository.CountAsync();
        var reservations = await _reservationRepository.GetActiveByDateRangeAsync(from, to);

        var bookedRooms = reservations
            .Select(r => r.RoomId)
            .Distinct()
            .Count();

        var occupancyRate = roomTotal == 0
            ? 0d
            : (double)bookedRooms / roomTotal * 100d;

        return Result<OccupancyDto>.Success(new OccupancyDto
        {
            TotalRooms = roomTotal,
            BookedRooms = bookedRooms,
            OccupancyRate = occupancyRate
        });
    }
}
