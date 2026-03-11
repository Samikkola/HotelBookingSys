using HotelBookingSys.Application.Interfaces;
using HotelBookingSys.Domain.Entities;
using HotelBookingSys.Application.DTOs;
using System;
using System.Collections.Generic;
using System.Text;

namespace HotelBookingSys.Application.UseCases;

public class GetAllRoomsUseCase
{
    private readonly IRoomRepository _roomRepository;

    public GetAllRoomsUseCase(IRoomRepository roomRepository)
    {
        _roomRepository = roomRepository;
    }

    public async Task<IEnumerable<RoomResponseDto>> ExecuteAsync()
    {
        var rooms = await _roomRepository.GetAllAsync();

        return rooms.Select(r => new RoomResponseDto
        {
            Id = r.Id,
            RoomNumber = r.RoomNumber,
            Type = r.Type.ToString(),
            RoomCapacity = r.RoomCapacity,
            BasePrice = r.BasePrice
        });
    }
}
