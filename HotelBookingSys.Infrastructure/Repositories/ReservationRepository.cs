using HotelBookingSys.Application.Interfaces;
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

        public async Task<IEnumerable<Reservation>> GetAllAsync()
        {
            return await _dbContext.Reservations.ToListAsync();
        }

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
    }
}