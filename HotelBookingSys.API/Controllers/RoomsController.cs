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

    public RoomsController(GetAllRoomsUseCase getAllRoomsUseCase)
    {
        _getAllRoomsUseCase = getAllRoomsUseCase;
    }

    [HttpGet]
    public async Task<ActionResult<IEnumerable<RoomResponseDto>>> GetAllRooms()
    {
        var rooms = await _getAllRoomsUseCase.ExecuteAsync();
        return Ok(rooms);
    }


}
