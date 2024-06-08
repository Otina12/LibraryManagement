namespace Library.Model.Abstractions.Errors;

public class AuthorErrors
{
    public static readonly Error AuthorNotFound = new("Author.AuthorNotFound", "Author does not exist.");
    public static readonly Error AuthorAlreadyExists = new("Author.AuthorAlreadyExists", "Author already exists.");
}
