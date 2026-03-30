namespace HotelBookingSys.Application.Common.Result;

/// <summary>
/// Represents the result of an operation, indicating success or failure, and containing error information if applicable.
/// Non-generic version of Result, used for operations that do not return a value.
/// </summary>
public class Result
{
    protected Result(bool isSuccess, ErrorCode errorCode, string? errorMessage)
    {
        IsSuccess = isSuccess;
        ErrorCode = errorCode;
        ErrorMessage = errorMessage;
    }

    public bool IsSuccess { get; }
    public bool IsFailure => !IsSuccess;
    public ErrorCode ErrorCode { get; }
    public string? ErrorMessage { get; }

    public static Result Success() => new(true, ErrorCode.None, null);

    public static Result Failure(ErrorCode errorCode, string? errorMessage = null)
        => new(false, errorCode, errorMessage);
}
