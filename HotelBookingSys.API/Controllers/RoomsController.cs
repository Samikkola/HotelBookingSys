using HotelBookingSys.Application.DTOs;
using HotelBookingSys.Application.UseCases;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSys.API.Controllers;


/// <summary>
/// Gets all rooms in the hotel.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class RoomsController : ControllerBase
{
    private readonly GetAllRoomsUseCase _getAllRoomsUseCase;
    private readonly GetAvailableRoomsUseCase _getAvailableRoomsUseCase;

    public RoomsController(GetAllRoomsUseCase getAllRoomsUseCase, GetAvailableRoomsUseCase getAvailableRoomsUseCase)
    {
        _getAllRoomsUseCase = getAllRoomsUseCase;
        _getAvailableRoomsUseCase = getAvailableRoomsUseCase;
    }

    [HttpGet("available")]
    public async Task<ActionResult<IEnumerable<RoomResponseDto>>> GetAvailableRooms([FromQuery] DateOnly checkInDate, [FromQuery] DateOnly checkOutDate)
    {
        var rooms = await _getAvailableRoomsUseCase.ExecuteAsync(checkInDate, checkOutDate);
        return Ok(rooms);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoomResponseDto>>> GetAllRooms()
    {
        var rooms = await _getAllRoomsUseCase.ExecuteAsync();
        return Ok(rooms);
    }


}
