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
    public async Task<IActionResult> Checkout(ReservationCheckoutDto reservationCheckoutDto)
    {
        reservationCheckoutDto = new ReservationCheckoutDto() // test data
        {
            ReservationId = new Guid("13652308-263c-409b-986c-08dca7d85416"),
            ReservationCopyCheckouts = new List<ReservationCopyCheckoutDto>()
            {
                new ReservationCopyCheckoutDto()
                {
                    ReservationCopyId = new Guid("6970F037-2092-4AF3-8E4E-08DCA7D8541F"),
                    BookCopyId = new Guid("47a72734-b723-45e4-005a-08dca71c7f45"),
                    NewStatus = Status.Normal
                },
                new ReservationCopyCheckoutDto()
                {
                    ReservationCopyId = new Guid("B13787AF-BAC8-43D3-8E4F-08DCA7D8541F"),
                    BookCopyId = new Guid("6ff128d6-92ce-43ad-005b-08dca71c7f45"),
                    NewStatus = Status.Normal
                }
            }
        };

        if (!ModelState.IsValid)
        {
            CreateFailureNotification("Something went wrong");
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
