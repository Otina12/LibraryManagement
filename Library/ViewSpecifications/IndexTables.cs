using Library.Service.Dtos.Author;
using Library.Service.Dtos.Book;
using Library.Service.Dtos.Publisher;
using Library.ViewModels;
using Library.ViewModels.Authors;
using Library.ViewModels.Publishers;

namespace Library.ViewSpecifications
{
    public class IndexTables
    {
        public static SortableTableModel GetBookTable(EntityFiltersDto<BookDto> bookListDto)
        {
            var sortableTableModel = new SortableTableModel
            {
                Items = bookListDto.Entities,
                SearchString = bookListDto.SearchString,
                SortBy = bookListDto.SortBy,
                SortOrder = bookListDto.SortOrder,
                PageNumber = bookListDto.PageNumber,
                PageSize = bookListDto.PageSize,
                TotalItems = bookListDto.TotalItems,
                Columns = new List<SortableColumn>
                            {
                            new("Title", "Title", true),
                            new("Edition", "Edition", false),
                            new("PublishYear", "Year", true),
                            new("Quantity", "Quantity", true),
                            new("PublisherName", "Publisher", false)
                            },
                ActionName = "Index",
                ControllerName = "Book",
                GetPropertyValue = (book, prop) =>
                {
                    var bookDto = book as BookDto;

                    return prop switch
                    {
                        "Id" => bookDto!.Id,
                        "Title" => bookDto!.Title,
                        "Edition" => bookDto!.Edition,
                        "PublishYear" => bookDto!.PublishYear.ToString(),
                        "Quantity" => bookDto!.Quantity.ToString(),
                        "PublisherName" => bookDto!.PublisherDto?.Name ?? "--------",
                        _ => ""
                    };
                }
            };

            return sortableTableModel;
        }

        public static SortableTableModel GetAuthorTable(EntityFiltersDto<AuthorDto> authorListDto)
        {
            var sortableTableModel = new SortableTableModel
            {
                Items = authorListDto.Entities,
                Columns = new List<SortableColumn>
                {
                    new("Name", "Name", true),
                    new("Email", "Email", false),
                    new("Period", "Period", true),
                    new("BooksCount", "# of Books in Library", false)
                },
                ActionName = "Index",
                ControllerName = "Author",
                GetPropertyValue = (author, prop) =>
                {
                    var authorDto = author as AuthorDto;
                    return prop switch
                    {
                        "Id" => authorDto!.Id,
                        "Name" => $"{authorDto!.Name} {authorDto.Surname}",
                        "Email" => authorDto!.Email ?? "--------",
                        "Period" => $"{authorDto!.BirthYear} - {(authorDto.DeathYear.HasValue ? authorDto.DeathYear.Value.ToString() : "")}",
                        "BooksCount" => authorDto!.Books.Length.ToString(),
                        _ => ""
                    };
                }
            };
            return sortableTableModel;
        }

        public static SortableTableModel GetPublisherTable(EntityFiltersDto<PublisherDto> publisherListDto)
        {
            var sortableTableModel = new SortableTableModel
            {
                Items = publisherListDto.Entities,
                Columns = new List<SortableColumn>
                {
                    new("Name", "Name", true),
                    new("Email", "Email", false),
                    new("PhoneNumber", "Phone Number", false),
                    new("YearPublished", "Year Published", true),
                    new("BooksCount", "# of Books in Library", false)
                },
                ActionName = "Index",
                ControllerName = "Publisher",
                GetPropertyValue = (publisher, prop) =>
                {
                    var publisherDto = publisher as PublisherDto;
                    return prop switch
                    {
                        "Id" => publisherDto!.Id,
                        "Name" => publisherDto!.Name,
                        "Email" => publisherDto!.Email ?? "--------",
                        "PhoneNumber" => publisherDto!.PhoneNumber ?? "--------",
                        "YearPublished" => publisherDto!.YearPublished.ToString(),
                        "BooksCount" => publisherDto!.Books.Length.ToString(),
                        _ => ""
                    };
                }
            };
            return sortableTableModel;
        }

    }
}
