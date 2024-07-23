using Library.Model.Models;

namespace Library.Model.Interfaces;

public interface IReservationRepository : IBaseModelRepository<Reservation>
{
    Task<IEnumerable<(DateTime, IEnumerable<Reservation>)>> GetAllIncompleteReservationsByDate(bool trackChanges = false);
    Task<IEnumerable<(DateTime, IEnumerable<Reservation>)>> GetAllCompleteReservationsByDate(bool trackChanges = false);
    Task<IEnumerable<(DateTime, IEnumerable<Reservation>)>> GetAllReservationsByDate(bool trackChanges = false);
    Task<IEnumerable<Reservation>> GetAllReservationsOfCustomer(string customerId, bool trackChanges = false);
    Task<IEnumerable<Reservation>> GetUpcomingReservationsOfCustomer(string customerId, bool trackChanges = false);
}
