using HotelBookingSys.Application.DTOs.RoomDtos;
using HotelBookingSys.Application.UseCases.Rooms;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSys.API.Controllers;


/// <summary>
/// Gets all rooms in the hotel.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class RoomsController : BaseController
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
        var result = await _getAvailableRoomsUseCase.ExecuteAsync(checkInDate, checkOutDate);
        return ToActionResult(result);
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoomResponseDto>>> GetAllRooms()
    {
        var result = await _getAllRoomsUseCase.ExecuteAsync();
        return ToActionResult(result);
    }


}
