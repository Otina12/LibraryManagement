namespace Library.Service.Dtos.Book.Get;

public record BookEditionDto(
    Guid Id,
    string ISBN,
    string PublisherName,
    int Edition,
    int Quantity
    )
{
    public int AvailableQuantity { get; set; }
}