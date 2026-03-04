using HotelBookingSys.Domain.Entities;


namespace HotelBookingSys.Domain.Repositories;

public interface IRoomRepository
{
    Task<Room> GetByRoomNumberAsync(int roomNumber);

    Task<Room> GetAvailableRooms(DateTime checkIndate, DateTime checkOutDate);
}


