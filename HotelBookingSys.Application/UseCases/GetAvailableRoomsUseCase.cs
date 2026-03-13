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
        var rooms = await _roomRepository.GetAllAsync();
        var availableRoomsDto = new List<RoomResponseDto>();

        // Check each room for availability
        foreach (var room in rooms)
        {
            var reservations = await _reservationRepository
                .GetOverlappingReservationsAsync(room.Id, checkInDate, checkOutDate);

            // A room is available if there are no overlapping reservations
            var isAvailable = !reservations.Any(r =>
                (checkInDate < r.CheckOutDate && checkOutDate > r.CheckInDate));
            // If the room is available, map it to dto add it to the list of available rooms
            if (isAvailable)
                availableRoomsDto.Add(MapToDto(room));
        }

        return availableRoomsDto;
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
