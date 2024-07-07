using AutoMapper;
using Library.Service.Dtos.Reservations.Post;
using Library.Service.Interfaces;
using Library.ViewModels.Reservations;
using Microsoft.AspNetCore.Mvc;

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
    public async Task<IActionResult> Create(CreateReservationViewModel reservationVM)
    {
        var valResult = Validate(reservationVM);
        if (valResult.IsFailure)
        {
            ViewBag.Books = await _serviceManager.BookService.GetAllBooksSorted();
            return View(reservationVM);
        }

        var reservationDto = _mapper.Map<CreateReservationDto>(reservationVM);
        var result = await _serviceManager.ReservationService.Create(reservationDto);

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
