using Library.Model.Models;

namespace Library.Model.Interfaces;

public interface IReservationRepository : IBaseModelRepository<Reservation>
{
    IEnumerable<IGrouping<DateTime, Reservation>> GetAllGroupedByDate(bool trackChanges = false);
}
