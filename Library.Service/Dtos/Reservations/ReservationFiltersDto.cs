using Library.Service.Dtos.Reservations.Get;

namespace Library.Service.Dtos.Reservations;

public class ReservationFiltersDto : EntityFiltersDto<(DateTime, IEnumerable<ReservationDto>)>
{
    public bool History { get; set; }
    public DateOnly MinReservationDate { get; set; }
    public DateOnly MaxReservationDate { get; set; }
    public DateOnly MinReturnDate { get; set; }
    public DateOnly MaxReturnDate { get; set; }
}
