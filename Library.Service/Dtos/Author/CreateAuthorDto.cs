namespace Library.Service.Dtos.Author;

public record CreateAuthorDto(
    string Name,
    string Surname,
    string? Email,
    string Description,
    int BirthYear,
    int? DeathYear
);
