using Library.Model.Abstractions;
using Library.Model.Abstractions.Errors;
using Library.Model.Interfaces;
using Library.Model.Models;
using Library.Model.Enums;
using Library.Service.Dtos.BookCopyLog.Post;
using Library.Service.Dtos.ReservationCopy.Get;
using Library.Service.Dtos.ReservationCopy.Post;
using Library.Service.Dtos.Reservations;
using Library.Service.Dtos.Reservations.Get;
using Library.Service.Dtos.Reservations.Post;
using Library.Service.Helpers.Mappers;
using Library.Service.Interfaces;
using Library.Service.Services.Logger;

namespace Library.Service.Services;

public class ReservationService : IReservationService
{
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILoggerManager _logger;

    public ReservationService(IUnitOfWork unitOfWork, ILoggerManager logger)
    {
        _unitOfWork = unitOfWork;
        _logger = logger;
    }

    public async Task<ReservationFiltersDto> GetAll(ReservationFiltersDto reservationFilters)
    {
        var reservationsByDate = reservationFilters.History ?
            await _unitOfWork.Reservations.GetAllReservationsByDate() :
            await _unitOfWork.Reservations.GetAllIncompleteReservationsByDate();

        reservationsByDate = FilterReservationsByReservationDate(reservationsByDate, reservationFilters.MinReservationDate, reservationFilters.MaxReservationDate);
        reservationsByDate = FilterReservationsByReturnDate(reservationsByDate, reservationFilters.MinReturnDate, reservationFilters.MaxReturnDate);
        reservationsByDate = SearchReservations(reservationsByDate, reservationFilters.SearchString!);
        reservationFilters.TotalItems = reservationsByDate.Count();
        reservationsByDate = PaginateReservations(reservationsByDate, reservationFilters.PageNumber, reservationFilters.PageSize);

        reservationFilters.Entities = reservationsByDate.Select(x => (
            x.Item1, // key: date
            x.Item2.Select(r => r.MapToReservationDto()) // value: reservations
            ))
            .ToList();

        return reservationFilters;
    }

    public async Task<Result<ReservationDetailsDto>> GetDetailsById(Guid Id)
    {
        var reservation = await _unitOfWork.Reservations.GetById(Id);

        if (reservation is null)
        {
            return Result.Failure(Error<ReservationDetailsDto>.NotFound);
        }

        var book = await _unitOfWork.Books.GetById(reservation.BookId);
        var customer = await _unitOfWork.Customers.GetById(reservation.CustomerId);

        var reservationDto = new ReservationDetailsDto(customer!, reservation.Id, reservation.SupposedReturnDate, book!, reservation.Quantity - reservation.ReturnedQuantity);

        // we need every book copy that was reserved for this reservation
        var reservationCopies = await _unitOfWork.ReservationCopies.GetAllReservationCopiesOfReservation(Id);
        reservationDto.ReservationCopies = reservationCopies.Select(
                    x => new ReservationCopyDto(x.Id, x.ReservationId, x.BookCopy.Id, x.TakenStatus, x.ReturnedStatus, x.ActualReturnDate, x.BookCopy.RoomId, x.BookCopy.ShelfId)
                ).ToList();

        // then get other (future) reservations of same customer:
        var otherReservations = await _unitOfWork.Reservations.GetUpcomingReservationsOfCustomer(reservationDto.Customer.Id);
        otherReservations = otherReservations.Where(x => x.Id != reservation.Id);

        foreach (var otherReservation in otherReservations)
        {
            reservationDto.OtherReservationsOfCustomer.Add(
                new ReservationDetailsDto(
                    customer!,
                    otherReservation.Id,
                    otherReservation.SupposedReturnDate,
                    (await _unitOfWork.Books.GetById(otherReservation.BookId))!,
                    otherReservation.Quantity - otherReservation.ReturnedQuantity
                ));
        }

        return reservationDto;
    }


    public async Task<Result> CreateReservation(string employeeId, CreateReservationDto createReservationDto)
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

    public async Task<Result> CheckoutReservation(ReservationCheckoutDto checkoutDto)
    {
        var reservation = await _unitOfWork.Reservations.GetById(checkoutDto.ReservationId, true);

        if (reservation is null)
        {
            return Result.Failure(Error<Reservation>.NotFound);
        }

        var result = await CheckoutReservationCopies(reservation, checkoutDto.ReservationCopyCheckouts);
        await _unitOfWork.SaveChangesAsync();
        return result;
    }

    // helpers
    private async Task<Result> CheckoutReservationCopies(Reservation reservation, List<ReservationCopyCheckoutDto> copyCheckouts)
    {
        foreach (var copyCheckout in copyCheckouts)
        {
            await CheckoutReservationCopy(copyCheckout, reservation.CustomerId);
        }

        reservation.ReturnedQuantity += copyCheckouts.Count;

        if (reservation.ReturnedQuantity == reservation.Quantity)
        {
            reservation.LastCopyReturnDate = DateTime.UtcNow;
        }

        return Result.Success();
    }

    private async Task CheckoutReservationCopy(ReservationCopyCheckoutDto copyCheckout, string customerId)
    {
        var reservationCopy = await _unitOfWork.ReservationCopies.GetById(copyCheckout.ReservationCopyId, true);
        var bookCopy = await _unitOfWork.BookCopies.GetById(copyCheckout.BookCopyId, true);

        if (bookCopy!.Status != copyCheckout.NewStatus)
        {
            _logger.LogInfo("Customer '{0}' changed status of BookCopy '{1}' from '{2}' to '{3}'",
                [customerId, bookCopy.Id, bookCopy.Status, copyCheckout.NewStatus]);    
        }

        reservationCopy!.ActualReturnDate = DateTime.UtcNow;
        reservationCopy!.ReturnedStatus = copyCheckout.NewStatus;

        bookCopy!.IsTaken = false;
        bookCopy!.Status = copyCheckout.NewStatus;

        var bookCopyLogDto = new CreateBookCopyLogDto(
            bookCopy.MapToBookCopyDto(),
            copyCheckout.NewStatus == BookCopyStatus.Lost ? BookCopyAction.Lost : BookCopyAction.Returned,
            copyCheckout.NewStatus == BookCopyStatus.Lost ? $"Book Copy Lost By Customer: {customerId}" : $"Book Copy returned By Customer: {customerId}",
            customerId);
        await _unitOfWork.BookCopyLogs.Create(bookCopyLogDto.MapToBookCopyLog());
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

            // add logs for all these copies
            var bookCopyLogsDto = quantityBookCopies
                .Select(x => new CreateBookCopyLogDto(x.MapToBookCopyDto(), BookCopyAction.Reserved, $"Book Copy reserved by Customer: {reservation.CustomerId}", reservation.CustomerId))
                .ToList();
            var bookCopyLogs = bookCopyLogsDto
                .Select(x => x.MapToBookCopyLog())
                .ToList();
            await _unitOfWork.BookCopyLogs.CreateRange(bookCopyLogs);

            await _unitOfWork.SaveChangesAsync(); // to save changes to book copies and logs after each iteration of loop
        }
        
        return reservationCopies;
    }

    private async Task<Result<List<Reservation>>> ValidateAndReturnReservations(string employeeId, CreateReservationDto createReservationDto)
    {
        var reservations = new List<Reservation>();

        foreach (var booksDto in createReservationDto.Books)
        {
            var originalBook = await _unitOfWork.OriginalBooks.GetById(booksDto.OriginalBookId);

            if (originalBook is null)
            {
                return Result.Failure<List<Reservation>>(Error<OriginalBook>.NotFound);
            }

            var bookResult = await ValidateAndGetBook(booksDto);
            if (bookResult.IsFailure)
                return Result.Failure<List<Reservation>>(bookResult.Error);

            var reservation = booksDto.MapToReservation(employeeId, createReservationDto.CustomerId);
            reservations.Add(reservation);
        }

        return Result.Success(reservations);
    }

    private async Task<Result<Book>> ValidateAndGetBook(BooksReservationDto bookDto)
    {
        var book = await _unitOfWork.Books.GetById(bookDto.BookId);

        if (book is null)
        {
            return Result.Failure<Book>(Error<Book>.NotFound);
        }

        var availableCopiesCount = await _unitOfWork.BookCopies.GetCountOfAvailableBookCopies(book.Id);

        if (availableCopiesCount < bookDto.Quantity)
        {
            return Result.Failure<Book>(BookErrors.NotEnoughCopies(book.OriginalBook.Title, book.Quantity));
        }

        return Result.Success(book);
    }


    // reservation filters
    private static IEnumerable<(DateTime, IEnumerable<Reservation>)> FilterReservationsByReservationDate(
    IEnumerable<(DateTime, IEnumerable<Reservation>)> groupedReservations, DateOnly minDate, DateOnly maxDate)
    {
        return groupedReservations
            .Select(group => (
                group.Item1,
                group.Item2.Where(reservation =>
                    DateOnly.FromDateTime(reservation.ReservationDate) >= minDate &&
                    DateOnly.FromDateTime(reservation.ReservationDate) <= maxDate)
            ))
            .Where(group => group.Item2.Any());
    }

    private static IEnumerable<(DateTime, IEnumerable<Reservation>)> FilterReservationsByReturnDate(
        IEnumerable<(DateTime, IEnumerable<Reservation>)> groupedReservations, DateOnly minDate, DateOnly maxDate)
    {
        if (groupedReservations.Any() && maxDate <= DateOnly.FromDateTime(DateTime.UtcNow)) // overdue reservations
        {
            var reservations = new List<Reservation>();

            foreach (var reservation in groupedReservations.First().Item2)
            {
                if (DateOnly.FromDateTime(reservation.SupposedReturnDate) <= maxDate)
                {
                    reservations.Add(reservation);
                }
            }

            return reservations.Any() ? [(DateTime.MinValue, reservations)] : [];
        }

        return groupedReservations
            .Where(x => DateOnly.FromDateTime(x.Item1) >= minDate &&
                        DateOnly.FromDateTime(x.Item1) <= maxDate)
            .ToList();
    }

    private static IEnumerable<(DateTime, IEnumerable<Reservation>)> SearchReservations(
        IEnumerable<(DateTime, IEnumerable<Reservation>)> groupedReservations, string searchString)
    {
        if (string.IsNullOrEmpty(searchString))
        {
            return groupedReservations;
        }

        searchString = searchString.Trim().ToLower();

        var result = groupedReservations.Select(x => (
            x.Item1,
            x.Item2.Where(r => r.CustomerId.Contains(searchString, StringComparison.CurrentCultureIgnoreCase) ||
                          r.Book.OriginalBook.Title.Contains(searchString, StringComparison.CurrentCultureIgnoreCase))
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
