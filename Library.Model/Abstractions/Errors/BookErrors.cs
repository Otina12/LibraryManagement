namespace Library.Model.Abstractions.Errors;

public static class BookErrors
{
    public static readonly Error BookNotFound = new("Book.BookNotFound", "Book does not exist.");
    public static readonly Error BookAlreadyExists = new("Book.BookAlreadyExists", "Book with given ISBN or Id already exists.");
    public static Error NotEnoughCopies(string title, int actualQuantity) => new("Book.NotEnoughCopies", $"Not enough copies for book {title}. Max: {actualQuantity}");
}
