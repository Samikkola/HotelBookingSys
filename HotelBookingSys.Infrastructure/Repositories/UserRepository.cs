using HotelBookingSys.Domain.Entities;
using HotelBookingSys.Domain.Interfaces;
using HotelBookingSys.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace HotelBookingSys.Infrastructure.Repositories;

public class UserRepository : IUserRepository
{
    private readonly ApplicationDbContext _dbContext;

    public UserRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }

    /// <summary>
    /// Retrieves a user by email address.
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public async Task<User?> GetByEmailAsync(string email)
    {
        return await _dbContext.Users.FirstOrDefaultAsync(x => x.Email == email);
    }

    /// <summary>
    /// Checks whether an email address already exists.
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    public async Task<bool> EmailExistsAsync(string email)
    {
        return await _dbContext.Users.AnyAsync(x => x.Email == email);
    }
}
