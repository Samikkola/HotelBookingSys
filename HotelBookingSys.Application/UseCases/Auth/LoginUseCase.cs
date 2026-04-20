using HotelBookingSys.Application.Common.Result;
using HotelBookingSys.Application.DTOs.AuthDtos;
using HotelBookingSys.Application.Interfaces;
using HotelBookingSys.Domain.Interfaces;

namespace HotelBookingSys.Application.UseCases.Auth;

public class LoginUseCase
{
    private readonly IUserRepository _userRepository;
    private readonly IJwtTokenService _jwtTokenService;

    public LoginUseCase(IUserRepository userRepository, IJwtTokenService jwtTokenService)
    {
        _userRepository = userRepository;
        _jwtTokenService = jwtTokenService;
    }

    /// <summary>
    /// Authenticates a user by email and password and returns a JWT auth result when valid.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    public async Task<Result<AuthResultDto>> ExecuteAsync(LoginDto request)
    {
        if (string.IsNullOrWhiteSpace(request.Email) || string.IsNullOrWhiteSpace(request.Password))
            return Result<AuthResultDto>.Failure(ErrorCode.Unauthorized, "Invalid email or password");

        var user = await _userRepository.GetByEmailAsync(request.Email.Trim());
        if (user is null)
            return Result<AuthResultDto>.Failure(ErrorCode.Unauthorized, "Invalid email or password");

        var isValidPassword = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
        if (!isValidPassword)
            return Result<AuthResultDto>.Failure(ErrorCode.Unauthorized, "Invalid email or password");

        return Result<AuthResultDto>.Success(new AuthResultDto
        {
            Token = _jwtTokenService.GenerateToken(user),
            ExpiresAt = DateTime.UtcNow.AddMinutes(60),
            Role = user.Role,
            FirstName = user.FirstName,
            LastName = user.LastName
        });
    }
}
