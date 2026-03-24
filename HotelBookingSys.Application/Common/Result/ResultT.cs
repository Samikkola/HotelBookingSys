namespace HotelBookingSys.Application.Common.Result;

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
