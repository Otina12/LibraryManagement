using Library.Model.Abstractions;
using Library.Model.Abstractions.Errors;
using Library.Model.Interfaces;
using Library.Model.Models;
using Library.Service.Dtos;
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

    public async Task<EntityFiltersDto<(DateTime, IEnumerable<ReservationDto>)>> GetAll(EntityFiltersDto<(DateTime, IEnumerable<ReservationDto>)> reservationFilters)
    {
        var reservationsByDate = await _unitOfWork.Reservations.GetAllByDate(false);

        reservationFilters.TotalItems = reservationsByDate.Count();
        reservationsByDate = SearchReservations(reservationsByDate, reservationFilters.SearchString);
        reservationsByDate = PaginateReservations(reservationsByDate, reservationFilters.PageNumber, reservationFilters.PageSize);

        reservationFilters.Entities = reservationsByDate.Select(x => (
            x.Item1, // date
            x.Item2.Select(r => r.MapToReservationDto())
            ));

        return reservationFilters;
    }

    public async Task<Result> CreateReservations(string employeeId, CreateReservationDto createReservationDto)
    {
        var reservationResult = await ValidateAndReturnReservations(employeeId, createReservationDto);
        if (reservationResult.IsFailure)
        {
            return reservationResult;
        }

        var reservations = reservationResult.Value();
        await _unitOfWork.Reservations.CreateRange(reservations);

        var reservationCopies = await GetReservationCopies(reservations);
        await _unitOfWork.ReservationCopies.CreateRange(reservationCopies);

        await _unitOfWork.SaveChangesAsync();
        return Result.Success();
    }

    private async Task<IEnumerable<ReservationCopy>> GetReservationCopies(List<Reservation> reservations)
    {
        var reservationCopies = new List<ReservationCopy>();

        foreach (var reservation in reservations)
        {
            var quantityBookCopies = await _unitOfWork.BookCopies.GetXBookCopies(reservation.BookId, reservation.Quantity, trackChanges: true);

            foreach (var bookCopy in quantityBookCopies)
            {
                bookCopy.IsTaken = true;

                reservationCopies.Add(new ReservationCopy
                {
                    ReservationId = reservation.Id,
                    BookCopyId = bookCopy.Id
                });
            }
        }

        return reservationCopies;
    }

    private async Task<Result<List<Reservation>>> ValidateAndReturnReservations(string employeeId, CreateReservationDto createReservationDto)
    {
        var reservations = new List<Reservation>();

        foreach (var booksDto in createReservationDto.Books)
        {
            var bookResult = await ValidateAndGetBook(booksDto);
            if (bookResult.IsFailure)
            {
                return Result.Failure<List<Reservation>>(bookResult.Error);
            }

            var reservation = booksDto.MapToReservation(employeeId, createReservationDto.CustomerId);
            reservations.Add(reservation);
        }

        return Result.Success(reservations);
    }

    private async Task<Result<Book>> ValidateAndGetBook(BooksReservationDto bookDto)
    {
        var bookExistsResult = await _validationService.BookExists(bookDto.BookId, true);
        if (bookExistsResult.IsFailure)
        {
            return Result.Failure<Book>(bookExistsResult.Error);
        }

        var book = bookExistsResult.Value();
        if (book.Quantity < bookDto.Quantity)
        {
            return Result.Failure<Book>(BookErrors.NotEnoughCopies(book.Title, book.Quantity));
        }

        book.Quantity -= bookDto.Quantity; // decrement when booked
        return Result.Success(book);
    }

    private static IEnumerable<(DateTime, IEnumerable<Reservation>)> SearchReservations(
        IEnumerable<(DateTime, IEnumerable<Reservation>)> groupedReservations, string? searchString)
    {
        if (string.IsNullOrEmpty(searchString))
        {
            return groupedReservations;
        }

        searchString = searchString.Trim().ToLower();

        var result = groupedReservations.Select(x => (
            x.Item1,
            x.Item2.Where(r => r.CustomerId.Contains(searchString, StringComparison.CurrentCultureIgnoreCase) ||
                          r.Book.Title.Contains(searchString, StringComparison.CurrentCultureIgnoreCase))
        ));

        return result
            .Where(x => x.Item2.Any())
            .ToList(); // filter out empty dates to not show them in the final view
    }

    private static IEnumerable<(DateTime, IEnumerable<Reservation>)> PaginateReservations(
        IEnumerable<(DateTime, IEnumerable<Reservation>)> groupedReservations, int pageNumber, int pageSize)
    {
        return groupedReservations.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToList();
    }
}
