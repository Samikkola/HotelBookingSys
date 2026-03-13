using HotelBookingSys.Application.DTOs;
using HotelBookingSys.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBookingSys.Application.UseCases;

public class GetAvailableRoomsUseCase
{
    private readonly IRoomRepository _roomRepository;

    public GetAvailableRoomsUseCase(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public async Task<IEnumerable<RoomResponseDto>> ExecuteAsync(DateOnly checkInDate, DateOnly checkOutDate)
    {
        if (checkInDate >= checkOutDate)
        {
            throw new ArgumentException("Check-in date must be before check-out date.");
        }

        var availableRooms = await _roomRepository.GetAvailableRooms(checkInDate, checkOutDate);

        return availableRooms.Select(r => new RoomResponseDto
        {
            Id = r.Id,
            RoomNumber = r.RoomNumber,
            Type = r.Type.ToString(),
            RoomCapacity = r.RoomCapacity,
            BasePrice = r.BasePrice
        });
    }
}
