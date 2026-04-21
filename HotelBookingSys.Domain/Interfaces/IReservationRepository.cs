using HotelBookingSys.Domain.Entities;
using HotelBookingSys.Domain.Enums;

namespace HotelBookingSys.Domain.Interfaces;

public interface IReservationRepository
{
    /// <summary>
    /// Retrieves reservation by identifier.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task<Reservation?> GetByIdAsync(Guid id);

    /// <summary>
    /// Adds a reservation.
    /// </summary>
    /// <param name="reservation"></param>
    /// <returns></returns>
    Task AddAsync(Reservation reservation);

    /// <summary>
    /// Updates a reservation.
    /// </summary>
    /// <param name="reservation"></param>
    /// <returns></returns>
    Task UpdateAsync(Reservation reservation);

    /// <summary>
    /// Deletes a reservation.
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    Task DeleteAsync(Guid id);

    /// <summary>
    /// Retrieves reservations using optional filters.
    /// </summary>
    /// <param name="customerId"></param>
    /// <param name="roomId"></param>
    /// <param name="status"></param>
    /// <param name="fromDate"></param>
    /// <param name="toDate"></param>
    /// <returns></returns>
    Task<(IEnumerable<Reservation> Items, int TotalCount)> GetReservationsAsync(
        Guid? customerId = null,
        Guid? roomId = null,
        ReservationStatus? status = null,
        DateOnly? fromDate = null,
        DateOnly? toDate = null,
        int page = 1,
        int pageSize = 20);

    Task<IReadOnlyList<Reservation>> GetOverlappingReservationsByRoomIdAsync(Guid roomId, DateOnly checkInDate, DateOnly checkOutDate);

    Task<IReadOnlyList<Reservation>> GetAllOverlappingReservationsAsync(DateOnly checkInDate, DateOnly checkOutDate);

    /// <summary>
    /// Retrieves active reservations overlapping the specified date range.
    /// </summary>
    /// <param name="from"></param>
    /// <param name="to"></param>
    /// <returns></returns>
    Task<IReadOnlyList<Reservation>> GetActiveByDateRangeAsync(DateOnly from, DateOnly to);

    /// <summary>
    /// Retrieves completed reservations for the specified year.
    /// </summary>
    /// <param name="year"></param>
    /// <returns></returns>
    Task<IReadOnlyList<Reservation>> GetCompletedByYearAsync(int year);

    /// <summary>
    /// Checks whether customer has active reservations.
    /// </summary>
    /// <param name="customerId"></param>
    /// <returns></returns>
    Task<bool> HasActiveReservationsByCustomerIdAsync(Guid customerId);
}
