using AutoMapper;
using Library.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers;

public class BookController : BaseController
{
    public BookController(IServiceManager serviceManager, IMapper mapper) : base(serviceManager, mapper)
    {
    }

    public IActionResult Index()
    {
        //var books = _serviceManager.BookService.GetAllBooks();
        return View();
    }
}
