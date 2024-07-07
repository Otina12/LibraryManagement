using Library.Model.Models;

namespace Library.Model.Interfaces;

public interface IReservationRepository
{
    IOrderedQueryable<IGrouping<DateTime, Reservation>> GetAllGroupedByDate(bool trackChanges);
}
