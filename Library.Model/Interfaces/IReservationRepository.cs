using Library.Model.Models;

namespace Library.Model.Interfaces;

public interface IReservationRepository : IBaseModelRepository<Reservation>
{
    Task<IEnumerable<(DateTime, IEnumerable<Reservation>)>> GetAllIncompleteByDate(bool trackChanges = false);
    Task<IEnumerable<(DateTime, IEnumerable<Reservation>)>> GetAllCompleteByDate(bool trackChanges = false);
    Task<IEnumerable<(DateTime, IEnumerable<Reservation>)>> GetAllByDate(bool trackChanges = false);
    Task<IEnumerable<Reservation>> GetAllReservationsOfCustomer(string customerId, bool trackChanges = false);
    Task<IEnumerable<Reservation>> GetUpcomingReservationsOfCustomer(string customerId, bool trackChanges = false);
}
