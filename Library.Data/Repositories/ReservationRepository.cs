using Library.Model.Interfaces;
using Library.Model.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Data.Repositories;

public class ReservationRepository : BaseModelRepository<Reservation>, IReservationRepository
{
    public ReservationRepository(ApplicationDbContext context) : base(context)
    {
    }

    public async override Task<IEnumerable<Reservation>> GetAll(bool trackChanges = false)
    {
        return trackChanges ?
            await dbSet
                .Include(x => x.Book)
                .ToListAsync() :
            await dbSet
                .Include(x => x.Book)
                .AsNoTracking()
                .ToListAsync();
    }

    public async Task<IEnumerable<(DateTime, IEnumerable<Reservation>)>> GetAllByDate(bool trackChanges)
    {
        var reservations = await GetAll(trackChanges);

        var overdueReservations = reservations // group all overdue reservations together
            .Where(r => r.SupposedReturnDate < DateTime.Today)
            .OrderBy(r => r.SupposedReturnDate)
            .ToList();

        var normalReservations = reservations // group normal reservations by date and sort
            .Where(r => r.SupposedReturnDate >= DateTime.Today)
            .GroupBy(r => r.SupposedReturnDate)
            .OrderBy(g => g.Key)
            .Select(g => (g.Key, g.AsEnumerable()))
            .ToList();

        var groupedByDateReservations = new List<(DateTime Date, IEnumerable<Reservation> Reservations)>();

        if (overdueReservations.Any())
        {
            groupedByDateReservations.Add((DateTime.MinValue, overdueReservations));
        }

        groupedByDateReservations.AddRange(normalReservations);
        return groupedByDateReservations;
    }

    public async Task<IEnumerable<Reservation>> GetAllReservationsOfCustomer(string customerId, bool trackChanges = false)
    {
        return trackChanges ?
            await _context.Reservations
            .Where(x => x.CustomerId == customerId)
            .ToListAsync()
            :
            await _context.Reservations
            .AsNoTracking()
            .Where(x => x.CustomerId == customerId)
            .ToListAsync();
    }

    public async Task<IEnumerable<Reservation>> GetUpcomingReservationsOfCustomer(string customerId, bool trackChanges = false)
    {
        return trackChanges ?
            await _context.Reservations
            .Where(x => x.CustomerId == customerId && x.Quantity != x.ReturnedQuantity)
            .ToListAsync()
            :
            await _context.Reservations
            .AsNoTracking()
            .Where(x => x.CustomerId == customerId && x.Quantity != x.ReturnedQuantity)
            .ToListAsync();
    }
}
