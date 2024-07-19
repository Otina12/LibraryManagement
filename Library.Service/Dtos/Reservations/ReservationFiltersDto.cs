using Library.Service.Dtos.Reservations.Get;

namespace Library.Service.Dtos.Reservations;

public class ReservationFiltersDto : EntityFiltersDto<(DateTime, IEnumerable<ReservationDto>)>
{
    public DateOnly MinDate {  get; set; }
    public DateOnly MaxDate { get; set; }
}
