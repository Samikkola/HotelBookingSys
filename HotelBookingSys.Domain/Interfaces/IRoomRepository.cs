using HotelBookingSys.Domain.Entities;


namespace HotelBookingSys.Domain.Interfaces;

public interface IRoomRepository
{
    Task<Room?> GetByRoomNumberAsync(int roomNumber);

    Task<IEnumerable<Room>> GetAllAsync();   

    Task<Room?> GetByIdAsync(Guid id);

}


