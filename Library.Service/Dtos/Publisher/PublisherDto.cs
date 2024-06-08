namespace Library.Service.Dtos.Publisher;

public record PublisherDto(
    Guid Id,
    string Name,
    string? Email,
    string? PhoneNumber,
    int YearPublished,
    int BookCount,
    DateTime CreationDate
    );