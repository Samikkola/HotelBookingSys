using HotelBookingSys.Application.Common;
using HotelBookingSys.Application.Common.Result;
using HotelBookingSys.Application.DTOs.ReservationDtos;
using HotelBookingSys.Application.Mappings.Reservations;
using HotelBookingSys.Domain.Interfaces;

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
    public async Task<Result<PagedResult<ReservationResponseDto>>> ExecuteAsync(ReservationFilterDto? filter = null, int page = 1, int pageSize = 10)
    {
        if (page < 1)
            return Result<PagedResult<ReservationResponseDto>>.Failure(ErrorCode.Validation, "Page must be at least 1.");

        if (pageSize < 1 || pageSize > 100)
            return Result<PagedResult<ReservationResponseDto>>.Failure(ErrorCode.Validation, "PageSize must be between 1 and 100.");

        if (filter is not null && filter.FromDate.HasValue && filter.ToDate.HasValue && filter.FromDate > filter.ToDate)
            return Result<PagedResult<ReservationResponseDto>>.Failure(ErrorCode.Validation, "FromDate must be on or before ToDate.");

        var (reservations, totalCount) = await _reservationRepository.GetReservationsAsync(
            filter?.CustomerId,
            filter?.RoomId,
            filter?.Status,
            filter?.FromDate,
            filter?.ToDate,
            page,
            pageSize);

        var rooms = await _roomRepository.GetAllAsync();
        var roomMap = rooms.ToDictionary(r => r.Id, r => r.RoomNumber);

        var items = reservations
            .Select(r => ReservationMapper.ToResponseDto(r, roomMap))
            .OrderBy(r => r.RoomNumber)
            .ToList();

        return Result<PagedResult<ReservationResponseDto>>.Success(new PagedResult<ReservationResponseDto>
        {
            Items = items,
            Page = page,
            PageSize = pageSize,
            TotalCount = totalCount
        });
    }
}
