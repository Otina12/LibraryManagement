using Library.Model.Interfaces;
using Library.Model.Models;

namespace Library.Data.Repositories;

public class ReservationCopyRepository : GenericRepository<ReservationCopy>, IReservationCopyRepository
{
    public ReservationCopyRepository(ApplicationDbContext context) : base(context)
    {
    }
}
