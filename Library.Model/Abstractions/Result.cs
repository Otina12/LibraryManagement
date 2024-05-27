namespace Library.Model.Abstractions;

public class Result
{
    public bool IsSuccess { get; }
    public bool IsFailure { get; }
    public Error Error { get; }

    protected internal Result(bool isSuccess, Error error)
    {
        if(isSuccess && error != Error.None ||
            !isSuccess && error == Error.None)
        {
            throw new ArgumentException("Wrong Error", nameof(error));
        }

        IsSuccess = isSuccess;
        Error = error;
        IsFailure = !isSuccess;
    }

    public static Result Success() => new(true, Error.None);
    public static Result<TValue> Success<TValue>(TValue value) => new(value, true, Error.None);
    public static Result Failure(Error error) => new(false, error);
    public static Result<TValue> Failure<TValue>(Error error) => new(default, false, error);
    public static Result<TValue> Create<TValue>(TValue? value) => value is not null ? Success(value) : Failure<TValue>(Error.Null);
    
    public static implicit operator Result(Error error) => Failure(error);
}
