﻿using Library.Model.Enums;
using Library.Service.Dtos.BookCopy.Get;

namespace Library.Service.Dtos.Reservations.Get;

public record ReservationDetailsDto(string CustomerId, DateTime SupposedReturnDate, string BookTitle, int QuantityToReturn)
{
    public List<BookCopyDto> BookCopies { get; set; } = [];
    public List<ReservationDetailsDto> OtherReservationsOfCustomer { get; set; } = [];
}
