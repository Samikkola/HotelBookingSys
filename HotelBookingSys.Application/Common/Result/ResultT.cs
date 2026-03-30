namespace HotelBookingSys.Application.Common.Result;

/// <summary>
/// Represents the result of an operation, indicating success or failure, and containing error information if applicable.
/// Used for operations that return a value of type T. Inherits from the non-generic Result class to include success status and error information.
/// </summary>
/// <typeparam name="T"></typeparam>
public class Result<T> : Result
{
    private Result(T? value, bool isSuccess, ErrorCode errorCode, string? errorMessage)
        : base(isSuccess, errorCode, errorMessage)
    {
        Value = value;
    }

    public T? Value { get; }

    public static Result<T> Success(T value) => new(value, true, ErrorCode.None, null);

    public static new Result<T> Failure(ErrorCode errorCode, string? errorMessage = null)
        => new(default, false, errorCode, errorMessage);
}
