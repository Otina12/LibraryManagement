namespace Library.Service.Dtos.Book.Get;

public record BookIdTitleAndQuantityDto(
    Guid Id,
    string Title,
    int Quantity
    );