using AutoMapper;
using Library.Attributes.Authorization;
using Library.Model.Enums;
using Library.Service.Dtos.Book.Get;
using Library.Service.Dtos;
using Library.Service.Dtos.Reservations.Post;
using Library.Service.Interfaces;
using Library.ViewModels.Reservations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using System.Globalization;
using System.Security.Claims;
using Library.Service.Dtos.Reservations.Get;

namespace Library.Controllers;

public class ReservationController : BaseController
{
    public ReservationController(IServiceManager serviceManager, IMapper mapper) : base(serviceManager, mapper)
    {
    }

    public async Task<IActionResult> Index(string? searchString, int pageNumber = 1, int pageSize = 3) // display next 3 dates' tables on each table
    {
        // reservation page has different structure compared to other entities that share generic filtering, sorting and etc. so we need to write it seperately
        var reservationParams = new EntityFiltersDto<(DateTime, IEnumerable<ReservationDto>)>
        {
            SearchString = searchString ?? "",
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var reservations = await _serviceManager.ReservationService.GetAll(reservationParams);
        return View(reservations);
    }

    public async Task<IActionResult> Create()
    {
        ViewBag.Books = await _serviceManager.BookService.GetAllBooksSorted();

        var viewModel = new CreateReservationViewModel();
        return View(viewModel);
    }

    [HttpPost]
    [CustomAuthorize($"{nameof(Role.Admin)},{nameof(Role.Librarian)}")]
    public async Task<IActionResult> Create([FromForm] CreateReservationViewModel reservationVM)
    {
        var valResult = Validate(reservationVM);
        if (valResult.IsFailure)
        {
            ViewBag.Books = await _serviceManager.BookService.GetAllBooksSorted();
            return View(reservationVM);
        }

        var curEmployeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var reservationDto = _mapper.Map<CreateReservationDto>(reservationVM);

        var result = await _serviceManager.ReservationService.CreateReservations(curEmployeeId!, reservationDto);

        return HandleResult(result, reservationVM, "Reservation has been confirmed", result.Error.Message, "Reservation");
    }

    public async Task<IActionResult> CustomerExists(string Id)
    {
        var customerDto = await _serviceManager.CustomerService.GetById(Id);

        if (customerDto is null)
        {
            return Json(new { success = false });
        }

        return Json(new { success = true, customerDto = new { name = $"{customerDto.Name} {customerDto.Surname}" } });
    }
}
