using AutoMapper;
using Library.Attributes.Authorization;
using Library.Model.Enums;
using Library.Service.Dtos.Reservations.Post;
using Library.Service.Interfaces;
using Library.ViewModels.Reservations;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace Library.Controllers;

public class ReservationController : BaseController
{
    public ReservationController(IServiceManager serviceManager, IMapper mapper) : base(serviceManager, mapper)
    {
    }

    public IActionResult Index()
    {
        return View();
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
        // test data
        reservationVM.Books = new List<BookCopiesViewModel>(){
            new BookCopiesViewModel{
                BookId = new Guid("94d1dcf9-1cf3-44f0-78cc-08dc953a6bd0"),
                Quantity = 6,
                SupposedReturnDate = DateOnly.FromDateTime(DateTime.Now.AddDays(10))
            }
        };

        var valResult = Validate(reservationVM);
        if (valResult.IsFailure)
        {
            ViewBag.Books = await _serviceManager.BookService.GetAllBooksSorted();
            return View(reservationVM);
        }

        var curEmployeeId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var reservationDto = _mapper.Map<CreateReservationDto>(reservationVM);
        
        var result = await _serviceManager.ReservationService.Create(curEmployeeId!, reservationDto);

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
