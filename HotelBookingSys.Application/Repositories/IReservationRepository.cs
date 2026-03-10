using HotelBookingSys.Domain.Entities;

namespace HotelBookingSys.Domain.Repositories;

public interface IReservationRepository
{
    Task<Reservation> GetByIdAsync(Guid id);
    Task AddAsync(Reservation reservation);
    Task UpdateAsync(Reservation reservation);
    Task DeleteAsync(Guid id);
    Task <IEnumerable<Reservation>> GetOverlappingReservationsAsync(Guid roomId, DateOnly checkInDate, DateOnly checkOutDate);
}
