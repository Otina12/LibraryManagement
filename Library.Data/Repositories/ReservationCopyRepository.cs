using Library.Model.Interfaces;
using Library.Model.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Data.Repositories;

public class ReservationCopyRepository : GenericRepository<ReservationCopy>, IReservationCopyRepository
{
    public ReservationCopyRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async Task<IEnumerable<BookCopy>> GetAllReservedBookCopiesOfReservation(Guid reservationId)
    {
        var bookCopiesIDs = await _context.ReservationCopies
            .Where(x => x.ReservationId == reservationId)
            .Select(x => x.BookCopyId)
            .ToListAsync();

        var bookCopies = new List<BookCopy>();

        foreach (var Id in bookCopiesIDs)
        {
            var bookCopy = await _context.BookCopies.FindAsync(Id);
            if (bookCopy is not null)
            {
                bookCopies.Add(bookCopy);
            }
        }

        return bookCopies;
    }

    public async Task<IEnumerable<ReservationCopy>> GetAllReservationCopiesOfReservation(Guid reservationId)
    {
        return await _context.ReservationCopies
                .Include(x => x.BookCopy)
                .AsNoTracking()
                .Where(x => x.ReservationId == reservationId)
                .ToListAsync();
    }

    public async Task<Dictionary<Reservation, IEnumerable<ReservationCopy>>> GetAllReservationCopiesOfReservations(IEnumerable<Reservation> reservations)
    {
        var reservationCopies = new Dictionary<Reservation, IEnumerable<ReservationCopy>>();

        foreach (var reservation in reservations)
        {
            var copies = await _context.ReservationCopies
                .AsNoTracking()
                .Where(x => x.ReservationId == reservation.Id)
                .ToListAsync();

            reservationCopies.Add(reservation, copies);
        }

        return reservationCopies;
    }

    // only difference from upper function is that we check if book copy is already returned
    public async Task<Dictionary<Reservation, IEnumerable<ReservationCopy>>> GetUpcomingReservationCopiesOfReservations(IEnumerable<Reservation> reservations)
    {
        var reservationCopies = new Dictionary<Reservation, IEnumerable<ReservationCopy>>();

        foreach (var reservation in reservations)
        {
            var copies = await _context.ReservationCopies
                .AsNoTracking()
                .Where(x => x.ReservationId == reservation.Id && x.ActualReturnDate == null)
                .ToListAsync();

            reservationCopies.Add(reservation, copies);
        }

        return reservationCopies;
    }
}
