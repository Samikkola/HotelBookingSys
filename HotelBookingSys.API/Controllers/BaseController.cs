using HotelBookingSys.Application.Common.Result;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace HotelBookingSys.API.Controllers;

[ApiController]
public abstract class BaseController : ControllerBase
{
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
