using Library.Model.Enums;
using System.ComponentModel.DataAnnotations.Schema;

namespace Library.Model.Models
{
    public class ReservationCopy
    {
        public Guid Id { get; set; }

        [ForeignKey(nameof(Reservation))]
        public Guid ReservationId { get; set; }
        // BookID and CustomerID will come from Reservation table
        [ForeignKey(nameof(BookCopy))]
        public Guid BookCopyId { get; set; }
        public string? ReturnCustomerId { get; set; }

        // SupposedReturnDate will come from Reservation table
        public DateTime? ActualReturnDate { get; set; }
        public Status? ReturnedStatus { get; set; }
        public Reservation Reservation { get; set; }
        public BookCopy BookCopy { get; set; }

    }
}
