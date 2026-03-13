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

    public ReservationsController(CreateReservationUseCase createReservationUseCase, GetReservationsUseCase getReservationsUseCase)
    {
        _createReservationUseCase = createReservationUseCase;
        _getReservationsUseCase = getReservationsUseCase;
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
}
