using HotelBookingSys.Domain.Entities;
using HotelBookingSys.Domain.Interfaces;
using HotelBookingSys.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSys.Infrastructure.Repositories;

public class ImageRepository : IImageRepository
{
    private readonly ApplicationDbContext _dbContext;

    public ImageRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    public async Task<RoomImage?> GetByIdAsync(Guid id)
    {
        return await _dbContext.RoomImages.FirstOrDefaultAsync(x => x.Id == id);
    }

    public async Task AddAsync(RoomImage image)
    {
        await _dbContext.RoomImages.AddAsync(image);
        await _dbContext.SaveChangesAsync();
    }

    public async Task DeleteAsync(RoomImage image)
    {
        _dbContext.RoomImages.Remove(image);
        await _dbContext.SaveChangesAsync();
    }

    public async Task<IEnumerable<RoomImage>> GetByRoomIdAsync(Guid roomId)
    {
        return await _dbContext.RoomImages
            .Where(x => x.RoomId == roomId)
            .OrderByDescending(x => x.CreatedAt)
            .ToListAsync();
    }
}
