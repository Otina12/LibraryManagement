using AutoMapper;
using Library.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers
{
    public class BookCopyController : BaseController
    {
        public BookCopyController(IServiceManager serviceManager, IMapper mapper) : base(serviceManager, mapper)
        {
        }

        [HttpGet]
        public async Task<IActionResult> Details(Guid Id)
        {
            var bookCopyLogsResult = await _serviceManager.BookCopyLogService.GetAllLogsOfBookCopy(Id);

            if (bookCopyLogsResult.IsFailure)
            {
                CreateFailureNotification($"Book Copy with ID: '{Id}' does not exist");
                return RedirectToAction("Index", "Reservation");
            }

            return View(bookCopyLogsResult.Value());
        }
    }
}
