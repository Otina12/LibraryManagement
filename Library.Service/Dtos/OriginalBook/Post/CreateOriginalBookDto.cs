namespace Library.Service.Dtos.OriginalBook.Post;

public record CreateOriginalBookDto(
    string Title,
    string? Description,
    int OriginalPublishYear,
    List<int> SelectedGenreIds);
