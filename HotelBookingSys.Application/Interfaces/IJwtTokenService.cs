using HotelBookingSys.Domain.Entities;

namespace HotelBookingSys.Application.Interfaces;

public interface IJwtTokenService
{
    /// <summary>
    /// Generates a JWT access token for the specified user.
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    string GenerateToken(User user);
}
