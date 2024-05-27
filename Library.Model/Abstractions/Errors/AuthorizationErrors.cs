namespace Library.Model.Abstractions.Errors;

public static class AuthorizationErrors
{
    public static readonly Error InvalidBirthday = new("Authorization.InvalidBirthday", "Birth date is invalid.");
    public static readonly Error EmployeeAlreadyExists = new("Authorization.EmployeeAlreadyExists", "Employee already exists.");
    public static readonly Error EmailNotFound = new("Authorization.EmailNotFound", "Employee with given email does not exist.");
    public static readonly Error WrongCredentials = new("Authorization.WrongCredentials", "Wrong credentials.");
    public static readonly Error UknownError = new("Authorization.UknownError", "An error occured. Please try again later.");
}
