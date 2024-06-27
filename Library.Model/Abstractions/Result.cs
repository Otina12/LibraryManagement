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
    public static Result<TValue> Success<TValue>(TValue value) => new Result<TValue>(value, true, Error.None);
    public static Result Failure(Error error) => new(false, error);
    public static Result<TValue> Failure<TValue>(Error<TValue> error) => new Result<TValue>(default, false, error);
    public static Result<TValue> Failure<TValue>(Error error) => new Result<TValue>(default, false, error);
    public static Result<TValue> Create<TValue>(TValue? value) => value is not null ? Success(value) : Failure<TValue>(Error.Null);

    public static implicit operator Result(Error error) => Failure(error);

    //public static implicit operator bool(Result result) => result.IsSuccess;
    //public static bool operator !(Result result) => result.IsFailure;
    public override string ToString() => IsSuccess ? "Success" : $"Failure: {Error}";
}


