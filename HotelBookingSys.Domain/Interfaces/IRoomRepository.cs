using HotelBookingSys.Domain.Entities;


namespace HotelBookingSys.Domain.Interfaces;

public interface IRoomRepository
{
    Task<Room?> GetByRoomNumberAsync(int roomNumber);

    Task<IReadOnlyList<Room>> GetAllAsync();

    Task<int> CountAsync();

    Task<Room?> GetByIdAsync(Guid id);

}


