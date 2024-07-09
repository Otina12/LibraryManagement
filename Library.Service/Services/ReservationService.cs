using Library.Model.Abstractions;
using Library.Model.Abstractions.Errors;
using Library.Model.Enums;
using Library.Model.Interfaces;
using Library.Model.Models;
using Library.Service.Dtos.Reservations.Get;
using Library.Service.Dtos.Reservations.Post;
using Library.Service.Helpers.Mappers;
using Library.Service.Interfaces;

namespace Library.Service.Services;

public class ReservationService : IReservationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly IValidationService _validationService;

    public ReservationService(IUnitOfWork unitOfWork, IValidationService validationService)
    {
        _unitOfWork = unitOfWork;
        _validationService = validationService;
    }

    public IEnumerable<(DateTime, IEnumerable<ReservationDto>)> GetAll()
    {
        var groupedReservations = _unitOfWork.Reservations.GetAllGroupedByDate();
        var reservations = groupedReservations.Select(x =>
            (
                x.Key,
                x.Select(r => r.MapToReservationDto())
            )
        );

        return reservations;
    }

    public async Task<Result> Create(string employeeId, CreateReservationDto createReservationDto)
    {
        var reservationResult = await ValidateAndCreateReservations(employeeId, createReservationDto);
        if (reservationResult.IsFailure)
        {
            return reservationResult;
        }

        await _unitOfWork.Reservations.CreateRange(reservationResult.Value());
        await _unitOfWork.SaveChangesAsync();
        return Result.Success();
    }

    private async Task<Result<List<Reservation>>> ValidateAndCreateReservations(string employeeId, CreateReservationDto createReservationDto)
    {
        var reservations = new List<Reservation>();

        foreach (var booksDto in createReservationDto.Books)
        {
            var bookResult = await ValidateAndGetBook(booksDto);
            if (bookResult.IsFailure)
            {
                return Result.Failure<List<Reservation>>(bookResult.Error);
            }

            var book = bookResult.Value();
            var bookCopies = await _unitOfWork.BookCopies.GetXBookCopies(book.Id, booksDto.Quantity);

            var newReservations = MapToReservations(bookCopies, createReservationDto, booksDto, employeeId);
            reservations.AddRange(newReservations);
        }

        return Result.Success(reservations);
    }

    private async Task<Result<Book>> ValidateAndGetBook(BookCopiesDto bookDto)
    {
        var bookExistsResult = await _validationService.BookExists(bookDto.BookId);
        if (bookExistsResult.IsFailure)
        {
            return Result.Failure<Book>(bookExistsResult.Error);
        }

        var book = bookExistsResult.Value();
        if (book.Quantity < bookDto.Quantity)
        {
            return Result.Failure<Book>(BookErrors.NotEnoughCopies(book.Title, book.Quantity));
        }

        return Result.Success(book);
    }

    // use only when booking
    private IEnumerable<Reservation> MapToReservations(IEnumerable<BookCopy> bookCopies, CreateReservationDto createReservationDto, BookCopiesDto bookDto, string employeeId)
    {
        var creationDate = DateTime.UtcNow;

        return bookCopies.Select(bookCopy => {
            bookCopy.IsTaken = true;

            return new Reservation
            {
                BookCopyId = bookCopy.Id,
                CustomerId = createReservationDto.CustomerId,
                ReservationDate = creationDate,
                SupposedReturnDate = bookDto.SupposedReturnDate.ToDateTime(new TimeOnly(23, 59)),
                EmployeeId = employeeId,
                CreationDate = creationDate
            };
        });
    }
}
