using AutoMapper;
using Library.Model.Enums;
using Library.Service.Dtos.Author;
using Library.Service.Dtos.Book;
using Library.Service.Dtos.Publisher;
using Library.Service.Interfaces;
using Library.ViewModels.Books;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Library.Controllers;

public class BookController : BaseController
{
    public BookController(IServiceManager serviceManager, IMapper mapper) : base(serviceManager, mapper)
    {
    }

    public async Task<IActionResult> Index(string searchString, string sortBy, string sortOrder, int pageNumber = 1, int pageSize = 5)
    {
        var booksParams = new BookListDto
        {
            SearchString = searchString,
            SortBy = sortBy,
            SortOrder = sortOrder,
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var books = await _serviceManager.BookService.GetAllFilteredBooks(booksParams);
        return View(books);
    }

    public async Task<IActionResult> Details(Guid id)
    {
        var book = await _serviceManager.BookService.GetBookById(id);
        return View(book.Value());
    }

    [HttpGet]
    public async Task<IActionResult> Create()
    {
        ViewBag.Publishers = await _serviceManager.PublisherService.GetAllPublisherIdAndNames();
        ViewBag.Authors = await _serviceManager.AuthorService.GetAllAuthorIdAndNames();
        ViewBag.Genres = await _serviceManager.GenreService.GetAllGenres();

        var roomShelfDictionary = await _serviceManager.ShelfService.GetRoomShelves();
        ViewBag.Rooms = roomShelfDictionary.Keys;
        ViewBag.Shelves = JsonSerializer.Serialize(roomShelfDictionary);

        return View(new CreateBookViewModel());
    }

    [HttpPost]
    public async Task<IActionResult> Create(CreateBookViewModel bookViewModel)
    {
        if (!ModelState.IsValid)
        {
            return View(bookViewModel);
        }

        var createBookDto = _mapper.Map<CreateBookDto>(bookViewModel);
        var createBookResult = await _serviceManager.BookService.CreateBook(createBookDto);

        if (createBookResult.IsFailure)
        {
            CreateFailureNotification(createBookResult.Error.Message);
            return View(bookViewModel);
        }

        CreateSuccessNotification("The book copies have been added succesfully");
        return RedirectToAction("Index", "Book", null);
    }
}
