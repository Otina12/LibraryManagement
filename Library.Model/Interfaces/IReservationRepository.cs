using Library.Model.Models;

namespace Library.Model.Interfaces;

public interface IReservationRepository : IBaseModelRepository<Reservation>
{
    Task<IEnumerable<(DateTime, IEnumerable<Reservation>)>> GetAllByDate(bool trackChanges = false);
}
