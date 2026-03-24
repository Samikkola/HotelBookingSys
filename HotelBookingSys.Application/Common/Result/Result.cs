namespace HotelBookingSys.Application.Common.Result;

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
