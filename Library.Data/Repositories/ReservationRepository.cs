using Library.Model.Interfaces;
using Library.Model.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Data.Repositories;

public class ReservationRepository : GenericRepository<Reservation>, IReservationRepository
{
    public ReservationRepository(ApplicationDbContext context) : base(context)
    {
    }

    public IOrderedQueryable<IGrouping<DateTime, Reservation>> GetAllGroupedByDate(bool trackChanges)
    {
        return trackChanges ?
            dbSet.GroupBy(x => x.SupposedReturnDate).Order() :
            dbSet.AsNoTracking().GroupBy(x => x.SupposedReturnDate).Order();
    }

}
