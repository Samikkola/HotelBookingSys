using HotelBookingSys.Application.Interfaces;
using HotelBookingSys.Domain.Entities;
using HotelBookingSys.Infrastructure;

namespace HotelBookingSys.Infrastructure.Repositories;

public class InMemoryReservationRepository : IReservationRepository
{
    private readonly InMemoryDatabase _database;

    public InMemoryReservationRepository(InMemoryDatabase database)
    {
        _database = database;
    }

    public Task AddAsync(Reservation reservation)
    {
        _database.Reservations.Add(reservation);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<Reservation> GetByIdAsync(Guid id)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Reservation>> GetOverlappingReservationsAsync(Guid roomId, DateOnly checkInDate, DateOnly checkOutDate)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<Reservation>> GetReservationsForRoomAsync(Guid roomId)
    {
        var reservations = _database.Reservations
            .Where(r => r.RoomId == roomId)
            .ToList();

        return Task.FromResult<IEnumerable<Reservation>>(reservations);
    }

    public Task UpdateAsync(Reservation reservation)
    {
        throw new NotImplementedException();
    }
}
