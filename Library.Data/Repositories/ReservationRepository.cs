using Library.Model.Interfaces;
using Library.Model.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Data.Repositories;

public class ReservationRepository : BaseModelRepository<Reservation>, IReservationRepository
{
    public ReservationRepository(ApplicationDbContext context) : base(context)
    {
    }

    public IEnumerable<IGrouping<DateTime, Reservation>> GetAllGroupedByDate(bool trackChanges)
    {
        return trackChanges ?
            dbSet
                .Include(x => x.BookCopy)
                .ThenInclude(x => x.Book)
                .GroupBy(x => x.SupposedReturnDate)
                .AsEnumerable()
                .OrderBy(x => x.Key) :
            dbSet
                .Include(x => x.BookCopy)
                .ThenInclude(x => x.Book)
                .AsNoTracking()
                .GroupBy(x => x.SupposedReturnDate)
                .AsEnumerable()
                .OrderBy(x => x.Key);
    }

}
