using HotelBookingSys.Application.DTOs;
using HotelBookingSys.Application.Interfaces;
using HotelBookingSys.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBookingSys.Application.UseCases;

public class GetAvailableRoomsUseCase
{
    private readonly IRoomRepository _roomRepository;
    private readonly IReservationRepository _reservationRepository;
    
    public GetAvailableRoomsUseCase(IRoomRepository roomRepository, IReservationRepository reservationRepository)
    {
        _roomRepository = roomRepository;
        _reservationRepository = reservationRepository;
    }

    public async Task<IEnumerable<RoomResponseDto>> ExecuteAsync(DateOnly checkInDate, DateOnly checkOutDate)
    {
        // Fetch everything needed concurrently
        var roomsTask = _roomRepository.GetAllAsync();
        var reservationsTask = _reservationRepository.GetAllOverlappingReservationsAsync(checkInDate, checkOutDate);
        // Wait for both tasks to complete
        await Task.WhenAll(roomsTask, reservationsTask);
        // Get results from tasks
        var rooms = roomsTask.Result;
        var overlappingReservations = reservationsTask.Result;

        // Filter in-memory using a HashSet of booked Room Ids for O(1) lookups
        var bookedRoomIds = overlappingReservations.Select(r => r.RoomId).ToHashSet();

        return rooms
            .Where(room => !bookedRoomIds.Contains(room.Id))
            .Select(MapToDto);
    }

    private RoomResponseDto MapToDto(Room room)
    {
        return new RoomResponseDto
        {
            RoomNumber = room.RoomNumber,
            RoomCapacity = room.RoomCapacity,
            Type = room.Type.ToString(),
            BasePrice = room.BasePrice,
        };
    }
}
