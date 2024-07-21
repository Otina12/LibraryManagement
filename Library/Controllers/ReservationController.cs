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
using Library.Service.Dtos.Reservations;
using Library.Service.Dtos.ReservationCopy.Post;

namespace Library.Controllers;

public class ReservationController : BaseController
{
    public ReservationController(IServiceManager serviceManager, IMapper mapper) : base(serviceManager, mapper)
    {
    }

    public async Task<IActionResult> Index(string? searchString, DateOnly? minReservationDate, DateOnly? maxReservationDate, DateOnly? minReturnDate, DateOnly? maxReturnDate, int pageNumber = 1, int pageSize = 3) // display next 3 dates' tables on each table
    {
        // reservation page has different structure compared to other entities that share generic filtering, sorting and etc. so we need to write it seperately
        var reservationParams = new ReservationFiltersDto
        {
            SearchString = searchString ?? "",
            PageNumber = pageNumber,
            PageSize = pageSize,
            MinReservationDate = minReservationDate ?? DateOnly.FromDateTime(DateTime.MinValue),
            MaxReservationDate = maxReservationDate ?? DateOnly.FromDateTime(DateTime.MaxValue),
            MinReturnDate = minReturnDate ?? DateOnly.FromDateTime(DateTime.MinValue),
            MaxReturnDate = maxReturnDate ?? DateOnly.FromDateTime(DateTime.MaxValue)
        };

        var reservations = await _serviceManager.ReservationService.GetAll(reservationParams);
        return View(reservations);
    }

    [CustomAuthorize($"{nameof(Role.Admin)},{nameof(Role.Librarian)}")]
    public IActionResult Create()
    {
        ViewBag.Books = _serviceManager.BookService.GetAllBooksSorted();

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
            ViewBag.Books = _serviceManager.BookService.GetAllBooksSorted();
            return View(reservationVM);
        }

        var curEmployeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var reservationDto = _mapper.Map<CreateReservationDto>(reservationVM);

        var result = await _serviceManager.ReservationService.CreateReservations(curEmployeeId!, reservationDto);

        return HandleResult(result, reservationVM, "Reservation has been confirmed", result.Error.Message, "Reservation");
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var reservationResult = await _serviceManager.ReservationService.GetDetailsById(id);

        if (reservationResult.IsFailure)
        {
            CreateFailureNotification($"Book with ID: '{id}' does not exist");
            return RedirectToAction("Index", "Reservation");
        }

        return View(reservationResult.Value());
    }

    [HttpPost]
    public async Task<IActionResult> Checkout([FromForm] ReservationCheckoutDto reservationCheckoutDto)
    {
        if (!ModelState.IsValid)
        {
            CreateFailureNotification("Invalid data passed");
            return RedirectToAction("Details", "Reservation", new {id = reservationCheckoutDto.ReservationId});
        }

        var checkoutResult = await _serviceManager.ReservationService.CheckoutReservation(reservationCheckoutDto);

        return HandleResult(checkoutResult, reservationCheckoutDto, "Checkout completed successfully", "Checkout failed", "Reservation");
    }

    public async Task<IActionResult> CustomerExists(string Id)
    {
        var customerDtoResult = await _serviceManager.CustomerService.GetCustomerById(Id);

        if (customerDtoResult.IsFailure)
        {
            return Json(new { success = false });
        }

        var customerDto = customerDtoResult.Value();
        return Json(new { success = true, customerDto = new { name = $"{customerDto.Name} {customerDto.Surname}" } });
    }
}
