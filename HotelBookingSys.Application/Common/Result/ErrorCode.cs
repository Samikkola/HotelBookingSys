namespace HotelBookingSys.Application.Common.Result;

/// <summary>
/// Defines error codes for operation results, allowing for standardized error handling across the application.
/// </summary>
public enum ErrorCode
{
    None = 0,
    NotFound,
    Validation,
    Conflict,
    Unauthorized,
    Forbidden,
    Unexpected
}
