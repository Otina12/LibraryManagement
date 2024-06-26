namespace Library.Service.Dtos.Book;

public record BookListDto
{
    public IEnumerable<BookDto> Books { get; set; }
    public string SearchString { get; set; }
    public string SortBy { get; set; }
    public string SortOrder { get; set; }
    public int PageNumber { get; set; }
    public int PageSize { get; set; }
    public int TotalItems { get; set; }
}