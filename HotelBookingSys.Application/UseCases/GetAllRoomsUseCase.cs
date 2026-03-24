using HotelBookingSys.Application.Common.Result;
using HotelBookingSys.Application.Interfaces;
using HotelBookingSys.Domain.Entities;
using HotelBookingSys.Application.DTOs;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBookingSys.Application.UseCases;

public class GetAllRoomsUseCase
{
    private readonly IRoomRepository _roomRepository;

    public GetAllRoomsUseCase(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public async Task<Result<IEnumerable<RoomResponseDto>>> ExecuteAsync()
    {
        var rooms = await _roomRepository.GetAllAsync();

        //TODO: Arrange by roomNumber 

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
