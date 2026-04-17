using HotelBookingSys.Application.Common.Result;
using HotelBookingSys.Domain.Interfaces;
using HotelBookingSys.Domain.Entities;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using HotelBookingSys.Application.DTOs.RoomDtos;

namespace HotelBookingSys.Application.UseCases.Rooms;

public class GetAllRoomsUseCase
{
    private readonly IRoomRepository _roomRepository;

    public GetAllRoomsUseCase(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    /// <summary>
    /// Retrieves all rooms from the repository. 
    /// Maps the domain entities to RoomResponseDto and returns a Result containing the list of rooms
    /// </summary>
    /// <returns></returns>
    public async Task<Result<IEnumerable<RoomResponseDto>>> ExecuteAsync()
    {
        var rooms = await _roomRepository.GetAllAsync();
        return Result<IEnumerable<RoomResponseDto>>.Success(rooms.Select(MapToDto).ToList());
    }

    private RoomResponseDto MapToDto(Room room)
    {
        return new RoomResponseDto
        {
            RoomNumber = room.RoomNumber,
            Type = room.Type.ToString(),
            RoomCapacity = room.RoomCapacity,
            BasePrice = room.BasePrice
        };
    }
}
