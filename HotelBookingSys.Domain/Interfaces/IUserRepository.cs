using HotelBookingSys.Domain.Entities;

namespace HotelBookingSys.Domain.Interfaces;

public interface IUserRepository
{
    /// <summary>
    /// Retrieves a user by email address.
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    Task<User?> GetByEmailAsync(string email);

    /// <summary>
    /// Checks if a user email already exists.
    /// </summary>
    /// <param name="email"></param>
    /// <returns></returns>
    Task<bool> EmailExistsAsync(string email);
}
