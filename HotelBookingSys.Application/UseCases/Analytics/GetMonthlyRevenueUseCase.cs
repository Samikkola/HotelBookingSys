using HotelBookingSys.Application.DTOs;
using HotelBookingSys.Application.Common.Result;
using HotelBookingSys.Domain.Interfaces;

namespace HotelBookingSys.Application.UseCases.Analytics;

public class GetMonthlyRevenueUseCase
{
    private readonly IReservationRepository _reservationRepository;

    public GetMonthlyRevenueUseCase(IReservationRepository reservationRepository)
    {
        _reservationRepository = reservationRepository;
    }

    /// <summary>
    /// Returns monthly revenue for completed reservations in the specified year.
    /// </summary>
    /// <param name="year"></param>
    /// <returns></returns>
    public async Task<Result<List<MonthlyRevenueDto>>> ExecuteAsync(int year)
    {
        if (year <= 0)
            return Result<List<MonthlyRevenueDto>>.Failure(ErrorCode.Validation, "Year must be greater than zero.");

        var completedReservations = await _reservationRepository.GetCompletedByYearAsync(year);

        var revenueByMonth = completedReservations
            .GroupBy(r => r.CheckInDate.Month)
            .Select(group => new MonthlyRevenueDto
            {
                Month = group.Key,
                TotalRevenue = group.Sum(r => r.TotalPrice)
            })
            .OrderBy(x => x.Month)
            .ToList();

        return Result<List<MonthlyRevenueDto>>.Success(revenueByMonth);
    }
}
