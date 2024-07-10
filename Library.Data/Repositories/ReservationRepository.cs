using Library.Model.Interfaces;
using Library.Model.Models;
using Microsoft.EntityFrameworkCore;

namespace Library.Data.Repositories;

public class ReservationRepository : BaseModelRepository<Reservation>, IReservationRepository
{
    public ReservationRepository(ApplicationDbContext context) : base(context)
    {
    }

    //public async override Task<IEnumerable<Reservation>> GetAll(bool trackChanges)
    //{
    //    return  trackChanges ?
    //        await dbSet
    //            .Include(x => x.BookCopy)
    //            .ThenInclude(x => x.Book)
    //            .ToListAsync() :
    //        await dbSet
    //            .Include(x => x.BookCopy)
    //            .ThenInclude(x => x.Book)
    //            .AsNoTracking()
    //            .ToListAsync();
    //}

}
