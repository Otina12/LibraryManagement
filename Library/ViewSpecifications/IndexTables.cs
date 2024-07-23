using Library.Service.Dtos.Author;
using Library.Service.Dtos;
using Library.Service.Dtos.Book.Get;
using Library.Service.Dtos.Customers.Get;
using Library.Service.Dtos.Publisher.Get;
using Library.ViewModels.Shared;

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
                IncludeDeleted = bookListDto.IncludeDeleted,
                TotalItems = bookListDto.TotalItems,
                Columns = new List<SortableColumn>
                            {
                            new("Title", "Title", true),
                            new("PublisherName", "Publisher", false),
                            new("Edition", "Edition", false),
                            new("PublishYear", "Year", true),
                            new("Quantity", "Quantity", true)
                            },
                ActionName = "Index",
                ControllerName = "Book",
                GetPropertyValue = (book, prop) =>
                {
                    var bookDto = book as BookDto;

                    return prop switch
                    {
                        "Id" => bookDto!.Id.ToString(),
                        "Title" => bookDto!.Title,
                        "PublisherName" => bookDto!.PublisherDto?.Name ?? "--------",
                        "Edition" => bookDto!.Edition,
                        "PublishYear" => bookDto!.PublishYear.ToString(),
                        "Quantity" => bookDto!.Quantity.ToString(),
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
                SearchString = authorListDto.SearchString,
                SortBy = authorListDto.SortBy,
                SortOrder = authorListDto.SortOrder,
                PageNumber = authorListDto.PageNumber,
                PageSize = authorListDto.PageSize,
                IncludeDeleted = authorListDto.IncludeDeleted,
                TotalItems = authorListDto.TotalItems,
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
                        "Id" => authorDto!.Id.ToString(),
                        "Name" => $"{authorDto!.Surname}, {authorDto!.Name}",
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
                SearchString = publisherListDto.SearchString,
                SortBy = publisherListDto.SortBy,
                SortOrder = publisherListDto.SortOrder,
                PageNumber = publisherListDto.PageNumber,
                PageSize = publisherListDto.PageSize,
                IncludeDeleted = publisherListDto.IncludeDeleted,
                TotalItems = publisherListDto.TotalItems,
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
                        "Id" => publisherDto!.Id.ToString(),
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

        public static SortableTableModel GetCustomerTable(EntityFiltersDto<CustomerDto> customerListDto)
        {
            var sortableTableModel = new SortableTableModel
            {
                Items = customerListDto.Entities,
                SearchString = customerListDto.SearchString,
                SortBy = customerListDto.SortBy,
                SortOrder = customerListDto.SortOrder,
                PageNumber = customerListDto.PageNumber,
                PageSize = customerListDto.PageSize,
                IncludeDeleted = customerListDto.IncludeDeleted,
                TotalItems = customerListDto.TotalItems,
                Columns = new List<SortableColumn>
                {
                    new("Name", "Name", true),
                    new("Id", "ID", false),
                    new("Email", "Email", false),
                    new("PhoneNumber", "Phone Number", false),
                    new("MembershipStartDate", "Member from", false)
                },
                ActionName = "Index",
                ControllerName = "Customer",
                GetPropertyValue = (customer, prop) =>
                {
                    var customerDto = customer as CustomerDto;
                    return prop switch
                    {
                        "Id" => customerDto!.Id.ToString(),
                        "Name" => $"{customerDto!.Surname}, {customerDto!.Name}",
                        "Email" => customerDto!.Email ?? "--------",
                        "PhoneNumber" => customerDto!.PhoneNumber ?? "--------",
                        "Address" => customerDto!.Address.ToString(),
                        "MembershipStartDate" => customerDto!.MembershipStartDate.ToString("dd MMMM yyyy"),
                        _ => ""
                    };
                }
            };
            return sortableTableModel;
        }

    }
}
