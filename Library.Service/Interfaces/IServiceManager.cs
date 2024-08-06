using Library.Model.Interfaces;

namespace Library.Service.Interfaces;

public interface IServiceManager
{
    IAuthenticationService AuthService { get; }
    IEmailSender EmailSender { get; }

    IEmployeeService EmployeeService { get; }
    INavMenuService NavMenuService { get; }
    IEmailService EmailService { get; }
    IPublisherService PublisherService { get; }
    IAuthorService AuthorService { get; }
    IOriginalBookService OriginalBookService { get; }
    IBookService BookService { get; }
    IGenreService GenreService { get; }
    IRoomService RoomService { get; }
    IShelfService ShelfService { get; }
    ICustomerService CustomerService { get; }
    IReservationService ReservationService { get; }
    IBookCopyLogService BookCopyLogService { get; }
}
