using HotelBookingSys.Application.DTOs;
using HotelBookingSys.Application.Common.Result;
using HotelBookingSys.Domain.Interfaces;

namespace HotelBookingSys.Application.UseCases.Analytics;

public class GetPopularRoomTypesUseCase
{
    private readonly IReservationRepository _reservationRepository;
    private readonly IRoomRepository _roomRepository;

    public GetPopularRoomTypesUseCase(IReservationRepository reservationRepository, IRoomRepository roomRepository)
    {
        _reservationRepository = reservationRepository;
        _roomRepository = roomRepository;
    }

    /// <summary>
    /// Returns room type popularity for reservations in the given date range.
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns></returns>
    public async Task<Result<List<RoomTypePopularityDto>>> ExecuteAsync(DateOnly from, DateOnly to)
    {
        if (from > to)
            return Result<List<RoomTypePopularityDto>>.Failure(ErrorCode.Validation, "From date must be on or before To date.");

        var reservations = await _reservationRepository.GetActiveByDateRangeAsync(from, to);
        var rooms = await _roomRepository.GetAllAsync();

        var roomsById = rooms.ToDictionary(r => r.Id);

        var popularity = reservations
            .Where(r => roomsById.ContainsKey(r.RoomId))
            .GroupBy(r => roomsById[r.RoomId].Type.ToString())
            .Select(group => new RoomTypePopularityDto
            {
                RoomType = group.Key,
                BookingCount = group.Count()
            })
            .OrderByDescending(x => x.BookingCount)
            .ToList();

        return Result<List<RoomTypePopularityDto>>.Success(popularity);
    }
}
