using HotelBookingSys.Application.Common.Result;
using HotelBookingSys.Application.DTOs.RoomDtos;
using HotelBookingSys.Application.Mappings.Rooms;
using HotelBookingSys.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

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
        return Result<IEnumerable<RoomResponseDto>>
            .Success(rooms.Select(RoomMapper.ToResponseDto)
            .OrderBy(r => r.RoomNumber)
            .ToList());
    }
}
