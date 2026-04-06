using HotelBookingSys.Application.Common.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSys.API.Controllers;

[ApiController]
public abstract class BaseController : ControllerBase
{
    /// <summary>
    /// Maps a non-generic result to an HTTP action result.
    /// </summary>
    /// <param name="result"></param>
    /// <returns></returns>
    protected ActionResult ToActionResult(Result result)
    {
        if (result.IsSuccess)
            return NoContent();

        return result.ErrorCode switch
        {
            ErrorCode.NotFound => NotFound(result.ErrorMessage),
            ErrorCode.Validation => BadRequest(result.ErrorMessage),
            ErrorCode.Conflict => Conflict(result.ErrorMessage),
            ErrorCode.Unauthorized => Unauthorized(result.ErrorMessage),
            ErrorCode.Forbidden => Forbid(),
            _ => StatusCode(StatusCodes.Status500InternalServerError, result.ErrorMessage)
        };
    }

    /// <summary>
    /// Maps a generic result to an HTTP action result.
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="result"></param>
    /// <returns></returns>
    protected ActionResult<T> ToActionResult<T>(Result<T> result)
    {
        if (result.IsSuccess)
            return Ok(result.Value);

        return result.ErrorCode switch
        {
            ErrorCode.NotFound => NotFound(result.ErrorMessage),
            ErrorCode.Validation => BadRequest(result.ErrorMessage),
            ErrorCode.Conflict => Conflict(result.ErrorMessage),
            ErrorCode.Unauthorized => Unauthorized(result.ErrorMessage),
            ErrorCode.Forbidden => Forbid(),
            _ => StatusCode(StatusCodes.Status500InternalServerError, result.ErrorMessage)
        };
    }
}
