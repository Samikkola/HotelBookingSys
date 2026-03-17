using HotelBookingSys.Domain.Entities;

namespace HotelBookingSys.Application.Interfaces;

public interface IReservationRepository
{
    Task<Reservation?> GetByIdAsync(Guid id);
    Task AddAsync(Reservation reservation);
    Task UpdateAsync(Reservation reservation);
    Task DeleteAsync(Guid id);
    Task<IEnumerable<Reservation>> GetAllAsync();
    Task <IReadOnlyList<Reservation>> GetOverlappingReservationsByRoomIdAsync(Guid roomId, DateOnly checkInDate, DateOnly checkOutDate);

    Task<IReadOnlyList<Reservation>> GetAllOverlappingReservationsAsync(DateOnly checkInDate, DateOnly checkOutDate);
}
