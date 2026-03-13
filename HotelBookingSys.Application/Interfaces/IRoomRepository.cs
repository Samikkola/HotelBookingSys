using HotelBookingSys.Domain.Entities;


namespace HotelBookingSys.Application.Interfaces;

public interface IRoomRepository
{
    Task<Room?> GetByRoomNumberAsync(int roomNumber);

    Task<IEnumerable<Room>> GetAvailableRooms(DateOnly checkIndate, DateOnly checkOutDate);

    Task<IEnumerable<Room>> GetAllAsync();   

    Task<Room?> GetByIdAsync(Guid id);

}


