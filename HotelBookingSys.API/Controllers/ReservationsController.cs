using HotelBookingSys.Application.Common.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HotelBookingSys.Application.UseCases.Reservations;
using HotelBookingSys.Application.DTOs.ReservationDtos;

namespace HotelBookingSys.API.Controllers;

/// <summary>
/// Creates a reservation for a specified customer and room within a given date range.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ReservationsController : BaseController
{
    private readonly CreateReservationUseCase _createReservationUseCase;
    private readonly GetReservationsUseCase _getReservationsUseCase;
    private readonly GetActiveReservationsByDateRangeUseCase _getActiveReservationsByDateRangeUseCase;
    private readonly CancelReservationUseCase _cancelReservationUseCase;
    private readonly UpdateReservationDatesUseCase _updateReservationDatesUseCase;
    private readonly CompleteReservationUseCase _completeReservationUseCase;

    public ReservationsController(
        CreateReservationUseCase createReservationUseCase, 
        GetReservationsUseCase getReservationsUseCase,
        GetActiveReservationsByDateRangeUseCase getActiveReservationsByDateRangeUseCase,
        CancelReservationUseCase cancelReservationUseCase,
        UpdateReservationDatesUseCase updateReservationDatesUseCase,
        CompleteReservationUseCase completeReservationUseCase)
    {
        _createReservationUseCase = createReservationUseCase;
        _getReservationsUseCase = getReservationsUseCase;
        _getActiveReservationsByDateRangeUseCase = getActiveReservationsByDateRangeUseCase;
        _cancelReservationUseCase = cancelReservationUseCase;
        _updateReservationDatesUseCase = updateReservationDatesUseCase;
        _completeReservationUseCase = completeReservationUseCase;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReservationResponseDto>>> GetReservations([FromQuery] DateOnly? from, [FromQuery] DateOnly? to)
    {
        if (from.HasValue || to.HasValue)
        {
            if (!from.HasValue || !to.HasValue)
                return ToActionResult(Result<IEnumerable<ReservationResponseDto>>.Failure(ErrorCode.Validation, "Both from and to dates are required, leave both empty to get all reservations."));

            var filteredResult = await _getActiveReservationsByDateRangeUseCase.ExecuteAsync(from.Value, to.Value);
            return ToActionResult(filteredResult);
        }

        var result = await _getReservationsUseCase.ExecuteAsync();
        return ToActionResult(result);
    }

    [HttpPost]
    public async Task<ActionResult<ReservationResponseDto>> CreateReservation([FromBody]CreateReservationDto request)
    {
        var result = await _createReservationUseCase.ExecuteAsync(request);
        if (result.IsSuccess)
            return CreatedAtAction(nameof(GetReservations), result.Value);

        return ToActionResult(result);
    }

    [HttpPut("{id}/cancel")]
    public async Task<ActionResult<ReservationResponseDto>> CancelReservation(Guid id)
    {
        var result = await _cancelReservationUseCase.ExecuteAsync(id);
        return ToActionResult(result);
    }

    [HttpPut("{id}/dates")]
    public async Task<ActionResult<ReservationResponseDto>> UpdateReservationDates(Guid id, [FromBody] UpdateReservationDatesDto request)
    {
        var result = await _updateReservationDatesUseCase.ExecuteAsync(id, request.NewCheckInDate, request.NewCheckOutDate);
        return ToActionResult(result);
    }
    [HttpPut("{id}/complete")]
    public async Task<ActionResult<ReservationResponseDto>> CompleteReservation(Guid id)
    {
        var result = await _completeReservationUseCase.ExecuteAsync(id);
        return ToActionResult(result);
    }

}
