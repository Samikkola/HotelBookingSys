using HotelBookingSys.Application.UseCases;
using Microsoft.AspNetCore.Mvc;
using HotelBookingSys.API.DTOs;

namespace HotelBookingSys.API.Controllers;

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
    public async Task<IActionResult> Create(CreateReservationRequestDto request)
    {
        var reservation = await _createReservationUseCase.ExecuteAsync(
            request.CustomerId,
            request.RoomNumber,
            request.CheckInDate,
            request.CheckOutDate
        );

        return Ok(reservation);
    }
}
