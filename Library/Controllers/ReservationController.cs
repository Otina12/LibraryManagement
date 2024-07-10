﻿using AutoMapper;
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

    public async Task<IActionResult> Index()
    {
        var reservations = await _serviceManager.ReservationService.GetAll();
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
        // test data
        reservationVM.Books = new List<BookCopiesViewModel>(){
            new BookCopiesViewModel{
                BookId = new Guid("1becd250-a65b-45d5-2668-08dc95454fa0"),
                Quantity = 4,
                SupposedReturnDate = DateOnly.FromDateTime(DateTime.Now.AddDays(3))
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
