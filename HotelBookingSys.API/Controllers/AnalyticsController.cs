using HotelBookingSys.Application.UseCases.Analytics;
using HotelBookingSys.Application.Common.Result;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using HotelBookingSys.Application.DTOs.AnalyticDtos;

namespace HotelBookingSys.API.Controllers;

/// <summary>
/// Provides business analytics endpoints for occupancy, revenue, and room popularity.
/// </summary>
[ApiController]
[Route("api/analytics")]
[Authorize(Roles = "Manager")]
public class AnalyticsController : BaseController
{
    private readonly GetOccupancyUseCase _getOccupancyUseCase;
    private readonly GetMonthlyRevenueUseCase _getMonthlyRevenueUseCase;
    private readonly GetPopularRoomTypesUseCase _getPopularRoomTypesUseCase;

    public AnalyticsController(
        GetOccupancyUseCase getOccupancyUseCase,
        GetMonthlyRevenueUseCase getMonthlyRevenueUseCase,
        GetPopularRoomTypesUseCase getPopularRoomTypesUseCase)
    {
        _getOccupancyUseCase = getOccupancyUseCase;
        _getMonthlyRevenueUseCase = getMonthlyRevenueUseCase;
        _getPopularRoomTypesUseCase = getPopularRoomTypesUseCase;
    }

    /// <summary>
    /// Returns occupancy rate for the given date range.
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns></returns>
    [HttpGet("occupancy")]
    public async Task<ActionResult<OccupancyDto>> GetOccupancy([FromQuery] DateOnly? from, [FromQuery] DateOnly? to)
    {
        
        if (!from.HasValue || !to.HasValue)
        {
            return ToActionResult(Result<OccupancyDto>.Failure(
                ErrorCode.Validation,
                "Both from and to dates are required."));
        }

        var result = await _getOccupancyUseCase.ExecuteAsync(from.Value, to.Value);
        return ToActionResult(result);
    }

    /// <summary>
    /// Returns monthly revenue for the specified year.
    /// </summary>
    /// <param name="year"></param>
    /// <returns></returns>
    [HttpGet("revenue")]
    public async Task<ActionResult<List<MonthlyRevenueDto>>> GetRevenue([FromQuery] int year)
    {
        var result = await _getMonthlyRevenueUseCase.ExecuteAsync(year);
        return ToActionResult(result);
    }

    /// <summary>
    /// Returns the most popular room types for the given date range.
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns></returns>
    [HttpGet("popular-room-types")]
    public async Task<ActionResult<List<RoomTypePopularityDto>>> GetPopularRoomTypes([FromQuery] DateOnly? from, [FromQuery] DateOnly? to)
    {
        if (!from.HasValue || !to.HasValue)
        {
            return ToActionResult(Result<List<RoomTypePopularityDto>>.Failure(
                ErrorCode.Validation,
                "Both from and to dates are required."));
        }

        var result = await _getPopularRoomTypesUseCase.ExecuteAsync(from.Value, to.Value);
        return ToActionResult(result);
    }
}
