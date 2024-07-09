using Library.Model.Models;

namespace Library.Model.Interfaces;

public interface IReservationRepository : IBaseModelRepository<Reservation>
{
    IOrderedQueryable<IGrouping<DateTime, Reservation>> GetAllGroupedByDate(bool trackChanges);
}
