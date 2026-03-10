using HotelBookingSys.Application.Interfaces;
using HotelBookingSys.Domain.Entities;
using HotelBookingSys.Infrastructure;

namespace HotelBookingSys.Infrastructure.Repositories;

public class InMemoryRoomRepository : IRoomRepository
{
    private readonly InMemoryDatabase _database;

    public InMemoryRoomRepository(InMemoryDatabase database)
    {
        _database = database;
    }

    public Task AddAsync(Room room)
    {
        _database.Rooms.Add(room);
        return Task.CompletedTask;
    }

    public Task<Room> GetAvailableRooms(DateTime checkIndate, DateTime checkOutDate)
    {
        throw new NotImplementedException();
    }

    public Task<Room?> GetByIdAsync(Guid id)
    {
        var room = _database.Rooms
            .FirstOrDefault(r => r.Id == id);

        return Task.FromResult(room);
    }

    public Task<Room?> GetByRoomNumberAsync(int roomNumber)
    {
        var room = _database.Rooms
            .FirstOrDefault(r => r.RoomNumber == roomNumber);

        return Task.FromResult(room);
    }
}
