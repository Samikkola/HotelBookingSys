using HotelBookingSys.Application.DTOs.AuthDtos;
using HotelBookingSys.Application.UseCases.Auth;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSys.API.Controllers;

/// <summary>
/// Provides authentication endpoints.
/// </summary>
[ApiController]
[Route("api/[controller]")]
public class AuthController : BaseController
{
    private readonly LoginUseCase _loginUseCase;

    public AuthController(LoginUseCase loginUseCase)
    {
        _loginUseCase = loginUseCase;
    }

    /// <summary>
    /// Authenticates a user and returns a JWT token result when credentials are valid.
    /// </summary>
    /// <param name="request"></param>
    /// <returns></returns>
    [HttpPost("login")]
    public async Task<ActionResult<AuthResultDto>> Login([FromBody] LoginDto request)
    {
        var result = await _loginUseCase.ExecuteAsync(request);
        return ToActionResult(result);
    }

    [HttpGet("authtest")]
    [Authorize(Roles = "Manager")]
    public async Task<ActionResult<string>> Test()
    {
        return "API is working!";
    }
}
