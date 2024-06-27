namespace Library.Model.Abstractions;

public class Error<T> : Error
{
    public static readonly Error<T> NotFound = new Error<T>("Entity.NotFound", $"{typeof(T).Name} not found.");
    public static readonly Error<T> AlreadyExists = new Error<T>("Entity.AlreadyExists", $"{typeof(T).Name} already exists.");

    public Error(string code, string message) : base(code, message)
    {
    }
}
