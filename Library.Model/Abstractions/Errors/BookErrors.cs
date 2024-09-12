namespace Library.Model.Abstractions.Errors;

public static class BookErrors
{
    public static Error NotEnoughCopies(string title, int actualQuantity) => new("Book.NotEnoughCopies", $"Not enough copies for book {title}. Max: {actualQuantity}");
}
