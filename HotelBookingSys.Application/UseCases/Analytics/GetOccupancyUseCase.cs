using HotelBookingSys.Application.Common.Result;
using HotelBookingSys.Domain.Interfaces;
using HotelBookingSys.Application.DTOs.AnalyticDtos;

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
        if (from >= to)
            return Result<OccupancyDto>.Failure(ErrorCode.Validation, "From date must be on or before To date.");

       
        var reservations = await _reservationRepository.GetActiveByDateRangeAsync(from, to);
        var totalRooms = await _roomRepository.CountAsync();

        var totalNights = to.DayNumber - from.DayNumber;
        var maxOccupancy = totalRooms * totalNights;
        // Calculate booked nights
        var bookedRoomNights = reservations.Sum(r =>
        {
            //Gets overlapping nights between reservation and date range
            var checkIn = r.CheckInDate < from ? from : r.CheckInDate;
            var checkOut = r.CheckOutDate > to ? to : r.CheckOutDate;
            return (checkOut.DayNumber - checkIn.DayNumber);
        });

        var occupancyRate = maxOccupancy == 0 ? 0 : (double)bookedRoomNights / maxOccupancy * 100;

        return Result<OccupancyDto>.Success(new OccupancyDto
        {
            TotalRooms = totalRooms,
            TotalNights = totalNights,
            BookedRoomNights = bookedRoomNights,
            OccupancyRate = occupancyRate
        });
    }
}
