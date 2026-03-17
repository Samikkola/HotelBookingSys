using HotelBookingSys.Application.Interfaces;
using HotelBookingSys.Domain.Entities;
using HotelBookingSys.Domain.Enums;
using HotelBookingSys.Infrastructure;

namespace HotelBookingSys.Infrastructure.Repositories;

public class InMemoryReservationRepository : IReservationRepository
{
    private readonly InMemoryDatabase _database;

    public InMemoryReservationRepository(InMemoryDatabase database)
    {
        _database = database;
    }

    public Task<IEnumerable<Reservation>> GetAllAsync()
    {
        return Task.FromResult<IEnumerable<Reservation>>(_database.Reservations);
    }

    public Task AddAsync(Reservation reservation)
    {
        _database.Reservations.Add(reservation);
        return Task.CompletedTask;
    }

    public Task DeleteAsync(Guid id)
    {
        var reservation = _database.Reservations.FirstOrDefault(r => r.Id == id);
        if (reservation != null)
        {
            reservation.CancelReservation(); // Soft delete
        }
        return Task.CompletedTask;
    }

    public Task<Reservation?> GetByIdAsync(Guid id)
    {
        var reservation = _database.Reservations.FirstOrDefault(r => r.Id == id);
        return Task.FromResult(reservation);
    }

    public Task<IReadOnlyList<Reservation>> GetOverlappingReservationsByRoomIdAsync(Guid roomId, DateOnly checkInDate, DateOnly checkOutDate)
    {
        //Gets all reservations for the specified room that overlap with the given check-in and check-out dates.
        var existingReservations = _database.Reservations
            .Where(r => r.RoomId == roomId &&
                        r.Status == ReservationStatus.Active &&
                        r.CheckInDate < checkOutDate &&
                        r.CheckOutDate > checkInDate)
            .ToList();
        return Task.FromResult<IReadOnlyList<Reservation>>(existingReservations);
    }

    public Task<IReadOnlyList<Reservation>> GetAllOverlappingReservationsAsync(DateOnly checkInDate, DateOnly checkOutDate)
    {
        var existingReservations = _database.Reservations
            .Where(r => r.Status == ReservationStatus.Active &&
                        r.CheckInDate < checkOutDate &&
                        r.CheckOutDate > checkInDate)
            .ToList();
        return Task.FromResult<IReadOnlyList<Reservation>>(existingReservations);
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
        var existingReservation = _database.Reservations.FirstOrDefault(r => r.Id == reservation.Id);
        if (existingReservation != null)
        {
            _database.Reservations.Remove(existingReservation);
            _database.Reservations.Add(reservation);
        }
        return Task.CompletedTask;
    }

}
