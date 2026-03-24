namespace HotelBookingSys.Application.Common.Result;

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
