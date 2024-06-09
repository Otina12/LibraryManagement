using AutoMapper;
using Library.Service.Dtos.Book;
using Library.Service.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace Library.Controllers;

public class BookController : BaseController
{
    public BookController(IServiceManager serviceManager, IMapper mapper) : base(serviceManager, mapper)
    {
    }

    public async Task<IActionResult> Index()
    {
        
        var books = await _serviceManager.BookService.GetAllBooks();
        return View(books);
    }
}
