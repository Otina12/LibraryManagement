namespace Library.Model.Abstractions.Errors;

public static class BookErrors
{
    public static readonly Error BookNotFound = new("Book.BookNotFound", "Book does not exist.");
    public static readonly Error BookAlreadyExists = new("Book.BookAlreadyExists", "Book already exists.");
    public static Error NotEnoughCopies(string title, int actualQuantity) => new("Book.NotEnoughCopies", $"Not enough copies for book {title}. Max: {actualQuantity}");
}
