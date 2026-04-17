using HotelBookingSys.Application.Common.Result;
using HotelBookingSys.Application.DTOs.RoomDtos;
using HotelBookingSys.Domain.Entities;
using HotelBookingSys.Domain.Interfaces;
using System;
using System.Buffers.Text;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HotelBookingSys.Application.UseCases.Rooms;

public class GetAvailableRoomsUseCase
{
    private readonly IRoomRepository _roomRepository;
    private readonly IReservationRepository _reservationRepository;
    
    public GetAvailableRoomsUseCase(IRoomRepository roomRepository, IReservationRepository reservationRepository)
    {
        _roomRepository = roomRepository;
        _reservationRepository = reservationRepository;
    }

    /// <summary>
    /// Retrieves a list of available rooms for the specified check-in and check-out dates.
    /// Fetches all rooms and overlapping reservations, then filters out rooms that are booked during the given date range.
    /// Returns a Result containing the list of available RoomResponseDto or error information if the operation fails.
    /// </summary>
    /// <param name="checkInDate"></param>
    /// <param name="checkOutDate"></param>
    /// <returns></returns>
    public async Task<Result<IEnumerable<RoomResponseDto>>> ExecuteAsync(DateOnly checkInDate, DateOnly checkOutDate)
    {
   
        var rooms = await _roomRepository.GetAllAsync();
        var overlappingReservations = await _reservationRepository.GetAllOverlappingReservationsAsync(checkInDate, checkOutDate);

        // Filter using a HashSet of booked Room Ids for O(1) lookups
        var bookedRoomIds = overlappingReservations.Select(r => r.RoomId).ToHashSet();

        // Return rooms that are not in the bookedRoomIds set, and map to DTOs
        var availableRooms = rooms
            .Where(room => !bookedRoomIds.Contains(room.Id))
            .Select(MapToDto)
            .OrderBy(r => r.RoomNumber);

        return Result<IEnumerable<RoomResponseDto>>.Success(availableRooms);
    }

    private RoomResponseDto MapToDto(Room room)
    {
        return new RoomResponseDto
        {
            Id = room.Id,
            RoomNumber = room.RoomNumber,
            RoomCapacity = room.RoomCapacity,
            Type = room.Type.ToString(),
            BasePrice = room.BasePrice,

            Images = room.Images
            .Select(i => new RoomImageResponseDto
            {
                Id = i.Id,
                Url = i.Url,           
            })
            .ToList()

        };
    }
}
