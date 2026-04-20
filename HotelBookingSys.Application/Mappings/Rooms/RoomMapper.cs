using HotelBookingSys.Application.DTOs.RoomDtos;
using HotelBookingSys.Domain.Entities;

namespace HotelBookingSys.Application.Mappings.Rooms;

public static class RoomMapper
{
    /// <summary>
    /// Maps room domain entity to room response DTO.
    /// </summary>
    /// <param name="room"></param>
    /// <returns></returns>
    public static RoomResponseDto ToResponseDto(Room room)
    {
        return new RoomResponseDto
        {
            Id = room.Id,
            RoomNumber = room.RoomNumber,
            Type = room.Type.ToString(),
            RoomCapacity = room.RoomCapacity,
            BasePrice = room.BasePrice,
            Images = room.Images
                .Select(i => new RoomImageResponseDto
                {
                    Id = i.Id,
                    Url = i.Url
                })
                .ToList()
        };
    }
}
