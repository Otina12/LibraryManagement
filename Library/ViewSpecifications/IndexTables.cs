using Library.Service.Dtos.Author;
using Library.Service.Dtos;
using Library.Service.Dtos.Book.Get;
using Library.Service.Dtos.Customers.Get;
using Library.Service.Dtos.Publisher.Get;
using Library.ViewModels.Shared;
using Library.Service.Dtos.OriginalBook.Get;
using Library.Service.Dtos.Report;
using Library.Model.Models.Report;

namespace Library.ViewSpecifications
{
    public class IndexTables
    {
        public static SortableTableModel GetOriginalBookTable(EntityFiltersDto<OriginalBookDto> originalBookListDto)
        {
            var originalBookTable = GetSortableTableModel(originalBookListDto);

            originalBookTable.Columns = new List<SortableColumn>()
            {
                new("Title", "Title", true),
                new("PublishYear", "Year", true),
                new("Quantity", "# of Book Editions in Library", false)
            };

            originalBookTable.ActionName = "Index";
            originalBookTable.ControllerName = "OriginalBook";
            originalBookTable.GetPropertyValue = (book, prop) =>
            {
                var originalBookDto = book as OriginalBookDto;

                return prop switch
                {
                    "Id" => originalBookDto!.Id,
                    "Title" => originalBookDto!.Title,
                    "PublishYear" => originalBookDto!.OriginalPublishYear.ToString(),
                    "Quantity" => originalBookDto!.Books.Length,
                    _ => ""
                }; ;
            };

            return originalBookTable;
        }
        public static SortableTableModel GetBookTable(EntityFiltersDto<BookDto> bookListDto)
        {
            var bookTable = GetSortableTableModel(bookListDto);

            bookTable.Columns = new List<SortableColumn>()
            {
            new("Title", "Title", true),
            new("PublisherName", "Publisher", false),
            new("Edition", "Edition", false),
            new("PublishYear", "Year", true),
            new("Quantity", "Quantity", true)
            };
            bookTable.ActionName = "Index";
            bookTable.ControllerName = "Book";
            bookTable.GetPropertyValue = (book, prop) =>
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
            };

            return bookTable;
        }

        public static SortableTableModel GetAuthorTable(EntityFiltersDto<AuthorDto> authorListDto)
        {
            var authorTable = GetSortableTableModel(authorListDto);

            authorTable.Columns = new List<SortableColumn>()
            {
                new("Name", "Name", true),
                new("Email", "Email", false),
                new("Period", "Period", true),
                new("BooksCount", "# of Books in Library", false)
            };
            authorTable.ActionName = "Index";
            authorTable.ControllerName = "Author";
            authorTable.GetPropertyValue = (author, prop) =>
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
            };

            return authorTable;
        }

        public static SortableTableModel GetPublisherTable(EntityFiltersDto<PublisherDto> publisherListDto)
        {
            var publisherTable = GetSortableTableModel(publisherListDto);

            publisherTable.Columns = new List<SortableColumn>()
            {
                new("Name", "Name", true),
                new("Email", "Email", false),
                new("PhoneNumber", "Phone Number", false),
                new("YearPublished", "Year Published", true),
                new("BooksCount", "# of Books in Library", false)
            };
            publisherTable.ActionName = "Index";
            publisherTable.ControllerName = "Publisher";
            publisherTable.GetPropertyValue = (publisher, prop) =>
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
            };

            return publisherTable;
        }

        public static SortableTableModel GetCustomerTable(EntityFiltersDto<CustomerDto> customerListDto)
        {
            var customerTable = GetSortableTableModel(customerListDto);

            customerTable.Columns = new List<SortableColumn>()
            {
                new("Name", "Name", true),
                new("Id", "ID", false),
                new("Email", "Email", false),
                new("PhoneNumber", "Phone Number", false),
                new("MembershipStartDate", "Member from", false)
            };
            customerTable.ActionName = "Index";
            customerTable.ControllerName = "Customer";
            customerTable.GetPropertyValue = (customer, prop) =>
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
            };

            return customerTable;
        }
        
        // generic properties that all types share
        private static SortableTableModel GetSortableTableModel<T>(EntityFiltersDto<T> entitiyFilters) where T : class
        {
            return new SortableTableModel
            {
                Items = entitiyFilters.Entities,
                SearchString = entitiyFilters.SearchString,
                SortBy = entitiyFilters.SortBy,
                SortOrder = entitiyFilters.SortOrder,
                PageNumber = entitiyFilters.PageNumber,
                PageSize = entitiyFilters.PageSize,
                IncludeDeleted = entitiyFilters.IncludeDeleted,
                TotalItems = entitiyFilters.TotalItems
            };
        }
    }
}
