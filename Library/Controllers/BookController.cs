using Library.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers;

public class BookController : Controller
{
    private readonly IServiceManager _serviceManager;

    public BookController(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    public IActionResult Index()
    {
        //var books = _serviceManager.BookService.GetAllBooks();
        return View();
    }
}
