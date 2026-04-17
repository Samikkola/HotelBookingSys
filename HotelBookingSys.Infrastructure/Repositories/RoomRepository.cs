using HotelBookingSys.Domain.Interfaces;
using HotelBookingSys.Domain.Entities;
using HotelBookingSys.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSys.Infrastructure.Repositories
{
    public class RoomRepository : IRoomRepository
    {
        private readonly ApplicationDbContext _dbContext;

        public RoomRepository(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task<IReadOnlyList<Room>> GetAllAsync()
        {
            return await _dbContext.Rooms
                .Include(r=>r.Images)
                .ToListAsync();
        }

        public async Task<int> CountAsync()
        {
            return await _dbContext.Rooms.CountAsync();
        }

        public async Task<Room?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Rooms.Include(r => r.Images).FirstOrDefaultAsync(r => r.Id == id);
        }

        public async Task<Room?> GetByRoomNumberAsync(int roomNumber)
        {
            return await _dbContext.Rooms.FirstOrDefaultAsync(r => r.RoomNumber == roomNumber);
        }
    }
}
