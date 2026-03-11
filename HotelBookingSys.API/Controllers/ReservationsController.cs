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

    public ReservationsController(CreateReservationUseCase createReservationUseCase)
    {
        _createReservationUseCase = createReservationUseCase;
    }

    [HttpPost]
    public async Task<ActionResult<ReservationResponseDto>> CreateReservation([FromBody]CreateReservationDto request)
    {
        var reservation = await _createReservationUseCase.ExecuteAsync(request);
        return Ok(reservation);
    }
}
