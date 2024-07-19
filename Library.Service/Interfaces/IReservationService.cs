﻿using Library.Model.Abstractions;
using Library.Service.Dtos;
using Library.Service.Dtos.Reservations;
using Library.Service.Dtos.Reservations.Get;
using Library.Service.Dtos.Reservations.Post;

namespace Library.Service.Interfaces;

public interface IReservationService
{
    Task<ReservationFiltersDto> GetAll(ReservationFiltersDto reservationFilters);
    Task<Result> CreateReservations(string employeeId, CreateReservationDto createReservationDto);
    Task<Result<ReservationDetailsDto>> GetDetailsById(Guid Id);
}
