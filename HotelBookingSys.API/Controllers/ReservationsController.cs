using HotelBookingSys.Application.UseCases;
using Microsoft.AspNetCore.Mvc;
using HotelBookingSys.Application.DTOs;

namespace HotelBookingSys.API.Controllers;

/// <summary>
/// Creates a reservation for a specified customer and room within a given date range.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class ReservationsController : ControllerBase
{
    private readonly CreateReservationUseCase _createReservationUseCase;
    private readonly GetReservationsUseCase _getReservationsUseCase;
    private readonly CancelReservationUseCase _cancelReservationUseCase;
    private readonly UpdateReservationDatesUseCase _updateReservationDatesUseCase;
    private readonly CompleteReservationUseCase _completeReservationUseCase;

    public ReservationsController(
        CreateReservationUseCase createReservationUseCase, 
        GetReservationsUseCase getReservationsUseCase,
        CancelReservationUseCase cancelReservationUseCase,
        UpdateReservationDatesUseCase updateReservationDatesUseCase,
        CompleteReservationUseCase completeReservationUseCase)
    {
        _createReservationUseCase = createReservationUseCase;
        _getReservationsUseCase = getReservationsUseCase;
        _cancelReservationUseCase = cancelReservationUseCase;
        _updateReservationDatesUseCase = updateReservationDatesUseCase;
        _completeReservationUseCase = completeReservationUseCase;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<ReservationResponseDto>>> GetReservations()
    {
        var reservations = await _getReservationsUseCase.ExecuteAsync();
        return Ok(reservations);
    }

    [HttpPost]
    public async Task<ActionResult<ReservationResponseDto>> CreateReservation([FromBody]CreateReservationDto request)
    {
        var reservation = await _createReservationUseCase.ExecuteAsync(request);
        return Ok(reservation);
    }

    [HttpPut("{id}/cancel")]
    public async Task<ActionResult<ReservationResponseDto>> CancelReservation(Guid id)
    {
        var result = await _cancelReservationUseCase.ExecuteAsync(id);
        return Ok(result);
    }

    [HttpPut("{id}/dates")]
    public async Task<ActionResult<ReservationResponseDto>> UpdateReservationDates(Guid id, [FromBody] UpdateReservationDatesDto request)
    {
        var result = await _updateReservationDatesUseCase.ExecuteAsync(id, request.NewCheckInDate, request.NewCheckOutDate);
        return Ok(result);
    }
    [HttpPut("{id}/complete")]
    public async Task<ActionResult<ReservationResponseDto>> CompleteReservation(Guid id)
    {
        var result = await _completeReservationUseCase.ExecuteAsync(id);
        return Ok(result);
    }
}
