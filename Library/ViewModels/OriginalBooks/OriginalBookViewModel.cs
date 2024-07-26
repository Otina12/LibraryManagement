using Library.Service.Dtos.Book.Get;

namespace Library.ViewModels.OriginalBooks;

public class OriginalBookViewModel
{
    public Guid Id { get; set; }
    public string Title { get; set; }
    public string? Description { get; set; }
    public int OriginalPublishYear { get; set; }
    public DateTime CreationDate { get; set; }
    public bool isDeleted { get; set; }
    public BookIdAndTitleDto[] Books { get; set; } = [];
}
