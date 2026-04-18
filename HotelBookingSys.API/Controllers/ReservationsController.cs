using HotelBookingSys.Application.Common.Result;
using HotelBookingSys.Application.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using HotelBookingSys.Application.UseCases.Reservations;
using HotelBookingSys.Application.DTOs.ReservationDtos;
using HotelBookingSys.Domain.Enums;


namespace HotelBookingSys.API.Controllers;


[ApiController]
[Route("api/[controller]")]
public class ReservationsController : BaseController
{
    private readonly CreateReservationUseCase _createReservationUseCase;
    private readonly GetReservationsUseCase _getReservationsUseCase;
    private readonly GetReservationByIdUseCase _getReservationByIdUseCase;
    private readonly GetActiveReservationsByDateRangeUseCase _getActiveReservationsByDateRangeUseCase;
    private readonly CancelReservationUseCase _cancelReservationUseCase;
    private readonly UpdateReservationUseCase _updateReservationUseCase;
    private readonly CompleteReservationUseCase _completeReservationUseCase;

    public ReservationsController(
        CreateReservationUseCase createReservationUseCase, 
        GetReservationsUseCase getReservationsUseCase,
        GetReservationByIdUseCase getReservationByIdUseCase,
        GetActiveReservationsByDateRangeUseCase getActiveReservationsByDateRangeUseCase,
        CancelReservationUseCase cancelReservationUseCase,
        UpdateReservationUseCase updateReservationUseCase,
        CompleteReservationUseCase completeReservationUseCase)
    {
        _createReservationUseCase = createReservationUseCase;
        _getReservationsUseCase = getReservationsUseCase;
        _getReservationByIdUseCase = getReservationByIdUseCase;
        _getActiveReservationsByDateRangeUseCase = getActiveReservationsByDateRangeUseCase;
        _cancelReservationUseCase = cancelReservationUseCase;
        _updateReservationUseCase = updateReservationUseCase;
        _completeReservationUseCase = completeReservationUseCase;
    }

    /// <summary>
    /// Returns active reservations within the given date range.
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns></returns>
    [HttpGet("active")]
    public async Task<ActionResult<IEnumerable<ReservationResponseDto>>> GetActiveReservationsByDateRange(
        [FromQuery] DateOnly? from,
        [FromQuery] DateOnly? to)
    {
        if (!from.HasValue || !to.HasValue)
        {
            return ToActionResult(
                Result<IEnumerable<ReservationResponseDto>>.Failure(
                    ErrorCode.Validation,
                    "Both from and to dates are required."));
        }

        var result = await _getActiveReservationsByDateRangeUseCase.ExecuteAsync(from.Value, to.Value);
        return ToActionResult(result);
    }

    /// <summary>
    /// Returns a reservation by identifier.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpGet("{id}")]
    public async Task<ActionResult<ReservationResponseDto>> GetReservationById(Guid id)
    {
        var result = await _getReservationByIdUseCase.ExecuteAsync(id);
        return ToActionResult(result);
    }

    /// <summary>
    /// Returns reservations using optional query filters.
    /// </summary>
    /// <param name="customerId"></param>
    /// <param name="roomId"></param>
    /// <param name="status"></param>
    /// <param name="fromDate"></param>
    /// <param name="toDate"></param>
    /// <returns></returns>
    [HttpGet]
    public async Task<ActionResult<PagedResult<ReservationResponseDto>>> GetReservations(
        [FromQuery] Guid? customerId,
        [FromQuery] Guid? roomId,
        [FromQuery] ReservationStatus? status,
        [FromQuery] DateOnly? fromDate,
        [FromQuery] DateOnly? toDate,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 10)
    {
        var filter = new ReservationFilterDto
        {
            CustomerId = customerId,
            RoomId = roomId,
            Status = status,
            FromDate = fromDate,
            ToDate = toDate
        };

        var result = await _getReservationsUseCase.ExecuteAsync(filter, page, pageSize);
        return ToActionResult(result);
    }

    /// <summary>
    /// Creates a reservation.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost]
    public async Task<ActionResult<ReservationResponseDto>> CreateReservation([FromBody]CreateReservationDto request)
    {
        var result = await _createReservationUseCase.ExecuteAsync(request);
        if (result.IsSuccess)
            return CreatedAtAction(nameof(GetReservationById), new { id = result.Value!.Id }, result.Value);

        return ToActionResult(result);
    }

    /// <summary>
    /// Cancels a reservation.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPut("{id}/cancel")]
    public async Task<ActionResult<ReservationResponseDto>> CancelReservation(Guid id)
    {
        var result = await _cancelReservationUseCase.ExecuteAsync(id);
        return ToActionResult(result);
    }

    /// <summary>
    /// Updates room, dates and guest count for a reservation.
    /// </summary>
    /// <param name="id"></param>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPut("{id}/dates")]
    public async Task<ActionResult<ReservationResponseDto>> UpdateReservationDates(Guid id, [FromBody] UpdateReservationDto request)
    {
        var result = await _updateReservationUseCase.ExecuteAsync(id, request);
        return ToActionResult(result);
    }

    /// <summary>
    /// Completes a reservation.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    [HttpPut("{id}/complete")]
    public async Task<ActionResult<ReservationResponseDto>> CompleteReservation(Guid id)
    {
        var result = await _completeReservationUseCase.ExecuteAsync(id);
        return ToActionResult(result);
    }

}
