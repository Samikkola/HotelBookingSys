using HotelBookingSys.Domain.Entities;


namespace HotelBookingSys.Application.Interfaces;

public interface IRoomRepository
{
    Task<Room?> GetByRoomNumberAsync(int roomNumber);

    Task<Room> GetAvailableRooms(DateTime checkIndate, DateTime checkOutDate);

    Task<IEnumerable<Room>> GetAllAsync();   

    Task<Room?> GetByIdAsync(Guid id);

}


