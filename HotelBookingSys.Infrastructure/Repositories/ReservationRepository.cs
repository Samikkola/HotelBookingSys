using HotelBookingSys.Domain.Interfaces;
using HotelBookingSys.Domain.Entities;
using HotelBookingSys.Domain.Enums;
using HotelBookingSys.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSys.Infrastructure.Repositories
{
    public class ReservationRepository : IReservationRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public ReservationRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task AddAsync(Reservation reservation)
        {
            await _dbContext.Reservations.AddAsync(reservation);
            await _dbContext.SaveChangesAsync();
        }

        public async Task DeleteAsync(Guid id)
        {
            var reservation = await _dbContext.Reservations.FindAsync(id);
            if (reservation != null)
            {
                reservation.CancelReservation(); // Soft delete
                await _dbContext.SaveChangesAsync();
            }
        }

        public async Task<(IEnumerable<Reservation> Items, int TotalCount)> GetReservationsAsync(
            Guid? customerId = null,
            Guid? roomId = null,
            ReservationStatus? status = null,
            DateOnly? fromDate = null,
            DateOnly? toDate = null,
            int page = 1,
            int pageSize = 20)
        {
            var query = _dbContext.Reservations.AsQueryable();

            if (customerId.HasValue)
                query = query.Where(r => r.CustomerId == customerId.Value);

            if (roomId.HasValue)
                query = query.Where(r => r.RoomId == roomId.Value);

            if (status.HasValue)
                query = query.Where(r => r.Status == status.Value);

            if (fromDate.HasValue)
                query = query.Where(r => r.CheckOutDate >= fromDate.Value);

            if (toDate.HasValue)
                query = query.Where(r => r.CheckInDate <= toDate.Value);

            var totalCount = await query.CountAsync();

            var items = await query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, totalCount);
        }

        public async Task<IReadOnlyList<Reservation>> GetActiveByDateRangeAsync(DateOnly from, DateOnly to)
        {
            return await _dbContext.Reservations
                .Where(r => (r.Status == ReservationStatus.Active || r.Status == ReservationStatus.Completed)
                            &&
                            r.CheckInDate < to &&
                            r.CheckOutDate > from)
                .ToListAsync();
        }

        public async Task<IReadOnlyList<Reservation>> GetCompletedByYearAsync(int year)
        {
            return await _dbContext.Reservations
                .Where(r => r.Status == ReservationStatus.Completed && r.CheckInDate.Year == year)
                .ToListAsync();
        }

        public Task<IReadOnlyList<Reservation>> GetActiveReservationsByDateRangeAsync(DateOnly from, DateOnly to)
            => GetActiveByDateRangeAsync(from, to);

        public async Task<IReadOnlyList<Reservation>> GetAllOverlappingReservationsAsync(DateOnly checkInDate, DateOnly checkOutDate)
        {
            return await _dbContext.Reservations
                .Where(r => r.Status == ReservationStatus.Active &&
                            r.CheckInDate < checkOutDate &&
                            r.CheckOutDate > checkInDate)
                .ToListAsync();
        }

        public async Task<Reservation?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Reservations.FindAsync(id);
        }

        public async Task<IReadOnlyList<Reservation>> GetOverlappingReservationsByRoomIdAsync(Guid roomId, DateOnly checkInDate, DateOnly checkOutDate)
        {
            return await _dbContext.Reservations
                .Where(r => r.RoomId == roomId &&
                            r.Status == ReservationStatus.Active &&
                            r.CheckInDate < checkOutDate &&
                            r.CheckOutDate > checkInDate)
                .ToListAsync();
        }

        public async Task UpdateAsync(Reservation reservation)
        {
            _dbContext.Reservations.Update(reservation);
            await _dbContext.SaveChangesAsync();
        }

        public async Task<bool> HasActiveReservationsByCustomerIdAsync(Guid customerId)
        {
            return await _dbContext.Reservations.AnyAsync(
                r => r.CustomerId == customerId && r.Status == ReservationStatus.Active);
        }
    }
}